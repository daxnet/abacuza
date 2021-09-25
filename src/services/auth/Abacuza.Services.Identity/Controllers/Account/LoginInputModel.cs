// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace Abacuza.Services.Identity.Controllers.Account
{
    /// <summary>
    /// Represents the login input model.
    /// </summary>
    public class LoginInputModel
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value which represents whether the login should be remembered.
        /// </summary>
        public bool RememberLogin { get; set; }

        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}