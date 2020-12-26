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

using System;

namespace Abacuza.Common
{
    /// <summary>
    /// Represents the error that occurs in Abacuza.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class AbacuzaException : Exception
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbacuzaException"/> class.
        /// </summary>
        public AbacuzaException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbacuzaException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AbacuzaException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbacuzaException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public AbacuzaException(string message, Exception innerException)
            : base(message, innerException)
        { }

        #endregion Public Constructors
    }
}