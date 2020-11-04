using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class TextAreaAttribute : UIComponent
    {
        public TextAreaAttribute(string name, string label)
            : base(name, label)
        {
        }

        public int MinLines { get; set; }
    }
}
