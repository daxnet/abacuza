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

using Abacuza.Endpoints;
using Microsoft.Spark.Sql;
using System;

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    /// <summary>
    /// Represents the generic base class for input readers.
    /// </summary>
    /// <typeparam name="TEndpoint">The type of the input endpoint.</typeparam>
    public abstract class InputReader<TEndpoint> : IInputReader
        where TEndpoint : IInputEndpoint
    {
        #region Public Methods

        /// <summary>
        /// Reads the data sets from the given <see cref="IInputEndpoint"/> and under the
        /// specified <see cref="SparkSession"/>.
        /// </summary>
        /// <param name="sparkSession">The <see cref="SparkSession"/> which creates the <see cref="DataFrame"/>.</param>
        /// <param name="inputEndpoint">The <see cref="IInputEndpoint"/> instance which provides the information
        /// of the input data sets.</param>
        /// <returns>The <see cref="DataFrame"/> for data processing.</returns>
        public DataFrame ReadFrom(SparkSession sparkSession, IInputEndpoint inputEndpoint)
        {
            if (sparkSession == null)
            {
                throw new ArgumentNullException(nameof(sparkSession));
            }

            if (inputEndpoint == null)
            {
                throw new ArgumentNullException(nameof(inputEndpoint));
            }

            if (inputEndpoint is TEndpoint endPoint)
            {
                return ReadFromInternal(sparkSession, endPoint);
            }

            throw new InputReaderException($"Input endpoint {inputEndpoint.GetType().FullName} can't be converted to type {typeof(TEndpoint).FullName}");
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Reads the data sets from the given <see cref="IInputEndpoint"/> and under the
        /// specified <see cref="SparkSession"/>.
        /// </summary>
        /// <param name="sparkSession">The <see cref="SparkSession"/> which creates the <see cref="DataFrame"/>.</param>
        /// <param name="inputEndpoint">The <see cref="IInputEndpoint"/> instance which provides the information
        /// of the input data sets.</param>
        /// <returns>The <see cref="DataFrame"/> for data processing.</returns>
        protected abstract DataFrame ReadFromInternal(SparkSession sparkSession, TEndpoint inputEndpoint);

        #endregion Protected Methods
    }
}