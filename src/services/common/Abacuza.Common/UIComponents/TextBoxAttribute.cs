using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class TextBoxAttribute : UIComponent
    {
        public TextBoxAttribute(string name, string label)
            : base(name, label)
        {
        }

        public int MaxLength { get; set; }

        public bool Required { get; set; }
    }
}
