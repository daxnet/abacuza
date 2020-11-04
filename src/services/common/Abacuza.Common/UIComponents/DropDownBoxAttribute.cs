using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class DropDownBoxAttribute : UIComponent
    {
        public DropDownBoxAttribute(string name, string label, string options)
            : base(name, label)
            => Options = options;

        public string Options { get; }
    }
}
