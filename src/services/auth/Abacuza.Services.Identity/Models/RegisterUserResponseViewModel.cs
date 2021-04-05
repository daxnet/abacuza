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

namespace Abacuza.Services.Identity.Models
{
    /// <summary>
    /// Represents the user registration response model.
    /// </summary>
    public class RegisterUserResponseViewModel
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>RegisterUserResponseViewModel</c> class.
        /// </summary>
        /// <param name="user">The user that is registered.</param>
        public RegisterUserResponseViewModel(AbacuzaAppUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            DisplayName = user.DisplayName;
            Email = user.Email;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string UserName { get; set; }

        #endregion Public Properties
    }
}