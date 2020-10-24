using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class DropDownBoxAttribute : UIComponent
    {
        public DropDownBoxAttribute(string label, string options)
            : base(label)
            => Options = options;

        public string Options { get; }
    }
}
