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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abacuza.Services.Identity.Models
{
    /// <summary>
    /// Represents the register user request model.
    /// </summary>
    public class RegisterUserRequestViewModel
    {

        #region Public Properties

        /// <summary>
        /// Gets or sets the display name of the registered user.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the role names.
        /// </summary>
        public List<string> RoleNames { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required]
        [StringLength(20)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString() => UserName;

        #endregion Public Methods
    }
}