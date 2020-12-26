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
// Licensed under LGPL-v3
// ==============================================================

using System.Collections.Generic;

namespace Abacuza.Common.UIComponents
{
    /// <summary>
    /// Represents the File Picker UI component.
    /// </summary>
    /// <seealso cref="Abacuza.Common.UIComponents.UIComponentAttribute" />
    /// <remarks>
    /// A File Picker UI component will render an Amazon S3 file list on the UI allowing
    /// users to upload local files to Amazon S3. Usually, the File Picker UI component
    /// should decorate a property of type <see cref="List{T}"/> in the containing class (where
    /// T is <c>S3File</c> instance).
    /// </remarks>
    public sealed class FilePickerAttribute : UIComponentAttribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePickerAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the UI component.</param>
        /// <param name="label">The label text of the UI component.</param>
        public FilePickerAttribute(string name, string label)
            : base(name, label)
        { }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets a list of allowed extensions, each of which is separated by comma (,).
        /// </summary>
        /// <value>
        /// The allowed extensions.
        /// </value>
        /// <remarks>For example: .csv,.tsv</remarks>
        public string AllowedExtensions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the File Uploader can upload multiple
        /// files once at a time.
        /// </summary>
        /// <value>
        ///   <c>true</c> if multiple files can be uploaded once at a time; otherwise, <c>false</c>.
        /// </value>
        public bool AllowMultipleSelection { get; set; }

        #endregion Public Properties
    }
}