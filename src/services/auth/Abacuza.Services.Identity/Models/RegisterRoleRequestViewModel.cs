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

using System.ComponentModel.DataAnnotations;

namespace Abacuza.Services.Identity.Models
{
    /// <summary>
    /// Represents the view model for registering the role.
    /// </summary>
    public class RegisterRoleRequestViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the role to be registered.
        /// </summary>
        [Required]
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the string representation of the role.
        /// </summary>
        /// <returns>The string representation of the role.</returns>
        public override string ToString() => Name;

        #endregion Public Methods
    }
}