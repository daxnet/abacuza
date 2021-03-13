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

using Abacuza.Common;
using System;

namespace Abacuza.JobRunners.Spark.SDK
{
    /// <summary>
    /// Represents the error that occurs when the spark runner is executing
    /// the data processing jobs.
    /// </summary>
    public sealed class SparkRunnerException : AbacuzaException
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>SparkRunnerException</c> class.
        /// </summary>
        public SparkRunnerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>SparkRunnerException</c> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public SparkRunnerException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <c>SparkRunnerException</c> class.
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The inner exception that caused this error to occur.</param>
        public SparkRunnerException(string message, Exception innerException)
            : base(message, innerException)
        { }

        #endregion Public Constructors
    }
}