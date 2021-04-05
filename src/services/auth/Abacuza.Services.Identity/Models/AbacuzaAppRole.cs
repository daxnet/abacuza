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

using Microsoft.AspNetCore.Identity;

namespace Abacuza.Services.Identity.Models
{
    /// <summary>
    /// Represents a user role in Abacuza.
    /// </summary>
    public class AbacuzaAppRole : IdentityRole
    {
        #region Public Properties

        /// <summary>
        /// Represents the description of the user role.
        /// </summary>
        public string Description { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the string representation of the user role.
        /// </summary>
        /// <returns>The string representation of the user role.</returns>
        public override string ToString() => Name;

        #endregion Public Methods
    }
}