using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class TextAttribute : UIComponent
    {
        public TextAttribute(string label)
            : base(label)
        {
        }

        public int MaxLength { get; set; }

        public bool Required { get; set; }
    }
}
