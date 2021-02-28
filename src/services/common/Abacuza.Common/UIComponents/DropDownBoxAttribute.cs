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
    /// Represents the drop-down box UI component.
    /// </summary>
    /// <seealso cref="Abacuza.Common.UIComponents.UIComponentAttribute" />
    public sealed class DropDownBoxAttribute : UIComponentAttribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownBoxAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the UI component.</param>
        /// <param name="label">The label of the UI component.</param>
        /// <param name="options">The options available for the drop-down box.</param>
        public DropDownBoxAttribute(string name, string label, string options)
            : base(name, label)
            => Options = options;

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the options separated with comma (,) that are available to be selected in a drop-down box.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public string Options { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Options: {Options}";

        #endregion Public Methods
    }
}