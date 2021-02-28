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

using Abacuza.Endpoints;
using Microsoft.Spark.Sql;
using System;

namespace Abacuza.JobRunners.Spark.SDK.OutputWriters
{
    /// <summary>
    /// Represents the output writers.
    /// </summary>
    /// <typeparam name="TEndpoint">The type of the output endpoint.</typeparam>
    public abstract class OutputWriter<TEndpoint> : IOutputWriter
        where TEndpoint : IOutputEndpoint
    {
        #region Public Methods

        /// <summary>
        /// Writes the data frame to the specified output endpoint.
        /// </summary>
        /// <param name="dataFrame">The <see cref="DataFrame"/> to be written to the output endpoint.</param>
        /// <param name="outputEndpoint">The output endpoint.</param>
        /// <param name="projectContext">The data that contains project and revision information.</param>
        public void WriteTo(DataFrame dataFrame, IOutputEndpoint outputEndpoint, ProjectContext projectContext)
        {
            if (dataFrame == null)
            {
                throw new ArgumentNullException(nameof(dataFrame));
            }

            if (outputEndpoint == null)
            {
                throw new ArgumentNullException(nameof(outputEndpoint));
            }

            if (projectContext == null)
            {
                throw new ArgumentNullException(nameof(projectContext));
            }

            if (outputEndpoint is TEndpoint endPoint)
            {
                WriteToInternal(dataFrame, endPoint, projectContext);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void WriteToInternal(DataFrame dataFrame, TEndpoint outputEndpoint, ProjectContext projectContext);

        #endregion Protected Methods
    }
}