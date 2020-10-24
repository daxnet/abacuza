using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class FilePickerAttribute : UIComponent
    {
        public FilePickerAttribute(string label)
            : base(label)
        { }

        public bool AllowMultipleSelection { get; set; }

        public string AllowedExtensions { get; set; }
    }
}
