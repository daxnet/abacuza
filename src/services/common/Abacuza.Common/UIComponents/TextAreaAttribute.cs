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
    /// Represents the Text Area UI component.
    /// </summary>
    /// <seealso cref="Abacuza.Common.UIComponents.UIComponentAttribute" />
    public sealed class TextAreaAttribute : UIComponentAttribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAreaAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the UI component.</param>
        /// <param name="label">The label text of the UI component.</param>
        public TextAreaAttribute(string name, string label)
            : base(name, label)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the minimum lines that should be shown by default
        /// in the Text Area component.
        /// </summary>
        /// <value>
        /// The minimum lines.
        /// </value>
        public int MinLines { get; set; }

        #endregion Public Properties
    }
}