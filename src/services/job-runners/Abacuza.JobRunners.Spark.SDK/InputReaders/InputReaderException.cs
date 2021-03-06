﻿// ==============================================================
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

namespace Abacuza.JobRunners.Spark.SDK.InputReaders
{
    /// <summary>
    /// Represents the error that occurs when data sets are read from input endpoints.
    /// </summary>
    public sealed class InputReaderException : AbacuzaException
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InputReaderException"/> class.
        /// </summary>
        public InputReaderException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputReaderException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InputReaderException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputReaderException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InputReaderException(string message, Exception innerException)
            : base(message, innerException)
        { }

        #endregion Public Constructors
    }
}