using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class UIComponent : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <c>UIAnnotation</c> class.
        /// </summary>
        /// <param name="label">The label text of the UI component.</param>
        protected UIComponent(string label) => Label = label;

        /// <summary>
        /// Gets the label text of the UI component on the front-end page.
        /// </summary>
        public string Label { get; }
    }
}
