﻿/*
 * Copyright (c) 2019 ETH Zürich, Educational Development and Technology (LET)
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

namespace SafeExamBrowser.ResetUtility.Procedure
{
	internal class Reset : ProcedureStep
	{
		public Reset(Context context) : base(context)
		{
		}

		internal override ProcedureStepResult Execute()
		{
			throw new NotImplementedException();
		}

		internal override ProcedureStep GetNextStep()
		{
			throw new NotImplementedException();
		}
	}
}