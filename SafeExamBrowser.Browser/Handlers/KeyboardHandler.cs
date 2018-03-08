﻿/*
 * Copyright (c) 2018 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Windows.Forms;
using CefSharp;
using BrowserSettings = SafeExamBrowser.Contracts.Configuration.Settings.BrowserSettings;

namespace SafeExamBrowser.Browser.Handlers
{
	/// <remarks>
	/// See https://cefsharp.github.io/api/63.0.0/html/T_CefSharp_IKeyboardHandler.htm.
	/// </remarks>
	internal class KeyboardHandler : IKeyboardHandler
	{
		private BrowserSettings settings;

		public KeyboardHandler(BrowserSettings settings)
		{
			this.settings = settings;
		}

		public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
		{
			return false;
		}

		public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
		{
			if (settings.AllowReloading && type == KeyType.KeyUp && windowsKeyCode == (int) Keys.F5)
			{
				browserControl.Reload();

				return true;
			}

			return false;
		}
	}
}