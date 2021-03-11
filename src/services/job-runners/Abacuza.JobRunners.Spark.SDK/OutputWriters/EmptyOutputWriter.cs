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
// Apache License Version 2.0
// ==============================================================

using Abacuza.Endpoints.Output;
using Abacuza.JobRunners.Spark.SDK.OutputWriters;
using Microsoft.Spark.Sql;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    /// <summary>
    /// Represents the output writer that doesn't write anything out.
    /// </summary>
    internal sealed class EmptyOutputWriter : OutputWriter<EmptyOutputEndpoint>
    {
        #region Protected Methods


        protected override void WriteToInternal(DataFrame dataFrame, EmptyOutputEndpoint outputEndpoint, ProjectContext projectContext)
        {
        }

        #endregion Protected Methods
    }
}