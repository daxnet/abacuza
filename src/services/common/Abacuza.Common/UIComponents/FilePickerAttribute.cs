using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common.UIComponents
{
    public sealed class FilePickerAttribute : UIComponentAttribute
    {
        public FilePickerAttribute(string name, string label)
            : base(name, label)
        { }

        public bool AllowMultipleSelection { get; set; }

        public string AllowedExtensions { get; set; }
    }
}
