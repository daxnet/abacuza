using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class TextAreaAttribute : UIComponent
    {
        public TextAreaAttribute(string label)
            : base(label)
        { }

        public int MinLines { get; set; }
    }
}
