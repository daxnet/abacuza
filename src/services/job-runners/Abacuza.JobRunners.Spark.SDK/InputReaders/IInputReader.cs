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

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    /// <summary>
    /// Represents that the implemented classes are input readers that read data sets
    /// from input endpoints and create the <see cref="DataFrame"/> for data processing.
    /// </summary>
    internal interface IInputReader
    {
        #region Public Methods

        /// <summary>
        /// Reads the data sets from the given <see cref="IInputEndpoint"/> and under the
        /// specified <see cref="SparkSession"/>.
        /// </summary>
        /// <param name="sparkSession">The <see cref="SparkSession"/> which creates the <see cref="DataFrame"/>.</param>
        /// <param name="inputEndpoint">The <see cref="IInputEndpoint"/> instance which provides the information
        /// of the input data sets.</param>
        /// <param name="context">The data that contains project and revision information.</param>
        /// <returns>The <see cref="DataFrame"/> for data processing.</returns>
        DataFrame ReadFrom(SparkSession sparkSession, IInputEndpoint inputEndpoint, ProjectContext context);

        #endregion Public Methods
    }
}