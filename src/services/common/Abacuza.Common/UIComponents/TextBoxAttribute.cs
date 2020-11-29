// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020 by daxnet. All rights reserved.
// Licensed under LGPL-v3
// ==============================================================

namespace Abacuza.Common.UIComponents
{
    /// <summary>
    /// Represents the Text Box UI component.
    /// </summary>
    public sealed class TextBoxAttribute : UIComponentAttribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>TextBoxAttribute</c> class.
        /// </summary>
        /// <param name="name">The name of the UI component.</param>
        /// <param name="label">The label of the UI component.</param>
        public TextBoxAttribute(string name, string label)
            : base(name, label)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the maximum allowed length of the text box.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value which indicates whether the value
        /// of the text box is required.
        /// </summary>
        public bool Required { get; set; }

        #endregion Public Properties
    }
}