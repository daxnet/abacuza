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

using System;

namespace Abacuza.Common.UIComponents
{
    /// <summary>
    /// Represents that the derived classes are UI components.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class UIComponentAttribute : Attribute
    {
        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <c>UIAnnotation</c> class.
        /// </summary>
        /// <param name="name">The name of the UI component.</param>
        /// <param name="label">The label text of the UI component.</param>
        protected UIComponentAttribute(string name, string label)
        {
            Name = name;
            Label = label;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the default value of the current UI component.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets the label text of the UI component on the front-end page.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets the name of the UI component.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the ordinal of the component in its container.
        /// </summary>
        public int Ordinal { get; set; } = 0;

        /// <summary>
        /// Gets or sets the tooltip of the component.
        /// </summary>
        public string Tooltip { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the string representation of the current <c>UIComponentAttribute</c> instance.
        /// </summary>
        /// <returns>The string representation of the current instance.</returns>
        public override string ToString() => Name;

        #endregion Public Methods
    }
}