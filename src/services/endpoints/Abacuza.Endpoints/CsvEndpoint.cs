// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Apache License Version 2.0
// ==============================================================

using Abacuza.Common.UIComponents;

namespace Abacuza.Endpoints
{
    public abstract class CsvEndpoint : Endpoint
    {

        #region Public Properties

        [TextBox("txtDateFormat",
                    "Date format",
                    DefaultValue = "yyyy-MM-dd",
                    MaxLength = 25,
                    Ordinal = 145,
                    Required = false,
                    Tooltip = "Sets the string that indicates a date format. Custom date formats follow the formats at java.text.SimpleDateFormat. This applies to date type.")]
        public string? DateFormat { get; set; }

        [TextBox("txtEncoding",
                    "Encoding",
                    DefaultValue = "UTF-8",
                    MaxLength = 20,
                    Ordinal = 185,
                    Required = false,
                    Tooltip = "Specifies the encoding of the input or output file.")]
        public string? Encoding { get; set; }

        [TextBox("txtEscape",
                    "Escape",
                    DefaultValue = "\\",
                    MaxLength = 1,
                    Ordinal = 175,
                    Required = false,
                    Tooltip = "Sets the single character used for escaping quotes inside an already quoted value.")]
        public string? Escape { get; set; }

        [Checkbox("chkHeader",
                    "Header",
                    DefaultValue = "true",
                    Ordinal = 187,
                    Tooltip = "Specifies whether the first line is for the names of columns.")]
        public bool Header { get; set; }

        [Checkbox("chkIgnoreLeadingWhiteSpace",
                    "Ignore leading white space",
                    DefaultValue = "true",
                    Ordinal = 160,
                    Tooltip = "Defines whether or not leading whitespaces from values should be skipped.")]
        public bool IgnoreLeadingWhiteSpace { get; set; }

        [Checkbox("chkIgnoreTrailingWhiteSpace",
                    "Ignore trailing white space",
                    DefaultValue = "false",
                    Ordinal = 155,
                    Tooltip = "Defines whether or not trailing whitespaces from values should be skipped.")]
        public bool IgnoreTrailingWhiteSpace { get; set; }

        [TextBox("txtLineSep",
            "Line separator",
            DefaultValue = "",
            Ordinal = 135,
            MaxLength = 1,
            Required = false,
            Tooltip = "Defines the line separator that should be used for parsing. If not set (empty), covers all \\r, \\r\\n and \\n.")]
        public string? LineSep { get; set; }

        [TextBox("txtNullValue",
                            "Null value",
                    DefaultValue = "",
                    MaxLength = 10,
                    Ordinal = 150,
                    Required = false,
                    Tooltip = "Sets the string representation of a null value.")]
        public string? NullValue { get; set; }

        [TextBox("txtQuote",
                    "Quote",
                    DefaultValue = "\"",
                    MaxLength = 1,
                    Ordinal = 180,
                    Required = false,
                    Tooltip = "Sets the single character used for escaping quoted values where the separator can be part of the value.")]
        public string? Quote { get; set; }

        /// <summary>
        /// Gets or sets the type of the separator of the row values.
        /// </summary>
        [DropDownBox("dropdownSeparator",
            "Delimiter",
            "Comma,Tab,Pipe,Space",
            Tooltip = "Chooses the delimiter used for separating the values.",
            Ordinal = 190,
            DefaultValue = "Comma")]
        public string? Separator { get; set; }

        [TextBox("txtTimestampFormat",
            "Timestamp format",
            DefaultValue = "yyyy-MM-dd'T'HH:mm:ss.SSSZZ",
            Ordinal = 140,
            MaxLength = 50,
            Required = false,
            Tooltip = "Sets the string that indicates a timestamp format. Custom date formats follow the formats at java.text.SimpleDateFormat. This applies to timestamp type.")]
        public string? TimestampFormat { get; set; }

        #endregion Public Properties
    }
}