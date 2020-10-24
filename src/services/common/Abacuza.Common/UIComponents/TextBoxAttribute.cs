using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class TextBoxAttribute : UIComponent
    {
        public TextBoxAttribute(string label)
            : base(label)
        {
        }

        public int MaxLength { get; set; }

        public bool Required { get; set; }
    }
}
