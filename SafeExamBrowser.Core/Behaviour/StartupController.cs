﻿/*
 * Copyright (c) 2017 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SafeExamBrowser.Contracts.Behaviour;
using SafeExamBrowser.Contracts.Configuration;
using SafeExamBrowser.Contracts.I18n;
using SafeExamBrowser.Contracts.Logging;
using SafeExamBrowser.Contracts.UserInterface;

namespace SafeExamBrowser.Core.Behaviour
{
	public class StartupController : IStartupController
	{
		private ILogger logger;
		private ISettings settings;
		private ISplashScreen splashScreen;
		private IText text;
		private IUiElementFactory uiFactory;

		private Stack<IOperation> stack = new Stack<IOperation>();

		public StartupController(ILogger logger, ISettings settings, IText text, IUiElementFactory uiFactory)
		{
			this.logger = logger;
			this.settings = settings;
			this.text = text;
			this.uiFactory = uiFactory;
		}

		public bool TryInitializeApplication(Queue<IOperation> operations)
		{
			try
			{
				Initialize(operations.Count);
				Perform(operations);
				Finish();

				return true;
			}
			catch (Exception e)
			{
				LogAndShowException(e);
				RevertOperations();
				Finish(false);

				return false;
			}
		}

		private void Perform(Queue<IOperation> operations)
		{
			foreach (var operation in operations)
			{
				stack.Push(operation);

				operation.SplashScreen = splashScreen;
				operation.Perform();

				splashScreen.Progress();

				// TODO: Remove!
				Thread.Sleep(250);
			}
		}

		private void RevertOperations()
		{
			while (stack.Any())
			{
				var operation = stack.Pop();

				operation.Revert();
				splashScreen.Regress();

				// TODO: Remove!
				Thread.Sleep(250);
			}
		}

		private void Initialize(int operationCount)
		{
			var titleLine = $"/* {settings.ProgramTitle}, Version {settings.ProgramVersion}{Environment.NewLine}";
			var copyrightLine = $"/* {settings.ProgramCopyright}{Environment.NewLine}";
			var emptyLine = $"/* {Environment.NewLine}";
			var githubLine = $"/* Please visit https://github.com/SafeExamBrowser for more information.";

			logger.Log($"{titleLine}{copyrightLine}{emptyLine}{githubLine}");
			logger.Log($"{Environment.NewLine}# Application started at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}{Environment.NewLine}");
			logger.Info("--- Initiating startup procedure ---");

			splashScreen = uiFactory.CreateSplashScreen(settings, text);
			splashScreen.SetMaxProgress(operationCount);
			splashScreen.UpdateText(Key.SplashScreen_StartupProcedure);
			splashScreen.InvokeShow();
		}

		private void LogAndShowException(Exception e)
		{
			logger.Error($"Failed to initialize application!", e);
			uiFactory.Show(text.Get(Key.MessageBox_StartupError), text.Get(Key.MessageBox_StartupErrorTitle), icon: MessageBoxIcon.Error);
			logger.Info("Reverting operations...");
		}

		private void Finish(bool success = true)
		{
			if (success)
			{
				logger.Info("--- Application successfully initialized! ---");
				logger.Log(string.Empty);
				splashScreen?.InvokeClose();
			}
			else
			{
				logger.Log($"{Environment.NewLine}# Application terminated at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
			}
		}
	}
}