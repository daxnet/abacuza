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

namespace Abacuza.Common.UIComponents
{
    /// <summary>
    /// Represents the Checkbox UI component.
    /// </summary>
    /// <seealso cref="Abacuza.Common.UIComponents.UIComponentAttribute" />
    public sealed class CheckBoxAttribute : UIComponentAttribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the UI component.</param>
        /// <param name="label">The label text of the UI component.</param>
        public CheckBoxAttribute(string name, string label)
            : base(name, label)
        { }

        #endregion Public Constructors
    }
}