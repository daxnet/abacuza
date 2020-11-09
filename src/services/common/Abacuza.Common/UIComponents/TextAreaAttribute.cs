using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class TextAreaAttribute : UIComponentAttribute
    {
        public TextAreaAttribute(string name, string label)
            : base(name, label)
        {
        }

        public int MinLines { get; set; }
    }
}
