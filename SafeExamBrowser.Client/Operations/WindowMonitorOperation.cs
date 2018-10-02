﻿/*
 * Copyright (c) 2018 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using SafeExamBrowser.Contracts.Configuration.Settings;
using SafeExamBrowser.Contracts.Core.OperationModel;
using SafeExamBrowser.Contracts.I18n;
using SafeExamBrowser.Contracts.Logging;
using SafeExamBrowser.Contracts.Monitoring;
using SafeExamBrowser.Contracts.UserInterface;

namespace SafeExamBrowser.Client.Operations
{
	internal class WindowMonitorOperation : IOperation
	{
		private KioskMode kioskMode;
		private ILogger logger;
		private IWindowMonitor windowMonitor;

		public IProgressIndicator ProgressIndicator { private get; set; }

		public WindowMonitorOperation(KioskMode kioskMode, ILogger logger, IWindowMonitor windowMonitor)
		{
			this.kioskMode = kioskMode;
			this.logger = logger;
			this.windowMonitor = windowMonitor;
		}

		public OperationResult Perform()
		{
			logger.Info("Initializing window monitoring...");
			ProgressIndicator?.UpdateText(TextKey.ProgressIndicator_InitializeWindowMonitoring);

			if (kioskMode == KioskMode.DisableExplorerShell)
			{
				windowMonitor.HideAllWindows();
			}

			if (kioskMode != KioskMode.None)
			{
				windowMonitor.StartMonitoringWindows();
			}

			return OperationResult.Success;
		}

		public OperationResult Repeat()
		{
			return OperationResult.Success;
		}

		public void Revert()
		{
			logger.Info("Stopping window monitoring...");
			ProgressIndicator?.UpdateText(TextKey.ProgressIndicator_StopWindowMonitoring);

			if (kioskMode != KioskMode.None)
			{
				windowMonitor.StopMonitoringWindows();
			}

			if (kioskMode == KioskMode.DisableExplorerShell)
			{
				windowMonitor.RestoreHiddenWindows();
			}
		}
	}
}