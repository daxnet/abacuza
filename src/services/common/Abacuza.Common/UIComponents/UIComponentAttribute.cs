using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
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

        public override string ToString() => Name;

        #endregion Public Methods
    }
}
