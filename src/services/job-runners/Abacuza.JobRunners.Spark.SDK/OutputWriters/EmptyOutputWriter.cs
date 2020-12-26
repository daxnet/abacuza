// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Licensed under LGPL-v3
// ==============================================================

using Abacuza.Endpoints.Output;
using Microsoft.Spark.Sql;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    /// <summary>
    /// Represents the output writer that doesn't write anything out.
    /// </summary>
    public sealed class EmptyOutputWriter : OutputWriter<EmptyOutputEndpoint>
    {
        #region Protected Methods


        protected override void WriteToInternal(DataFrame dataFrame, EmptyOutputEndpoint outputEndpoint)
        {
        }

        #endregion Protected Methods
    }
}