using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class FilesAttribute : UIComponent
    {
        public FilesAttribute(string label)
            : base(label)
        { }

        public bool AllowMultipleSelection { get; set; }

        public string AllowedExtensions { get; set; }
    }
}
