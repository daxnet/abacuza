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
    /// Represents the response model of the role creation.
    /// </summary>
    public class RegisterRoleResponseViewModel
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>RegisterRoleResponseViewModel</c> class.
        /// </summary>
        /// <param name="registeredRole">The registered role.</param>
        public RegisterRoleResponseViewModel(AbacuzaAppRole registeredRole)
            => (Id, Name, Description) = (registeredRole.Id, registeredRole.Name, registeredRole.Description);

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the description of the registered role.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the id of the registered role.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the name of the registered role.
        /// </summary>
        public string Name { get; }

        #endregion Public Properties
    }
}