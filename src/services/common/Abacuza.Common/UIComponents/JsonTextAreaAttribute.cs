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

namespace Abacuza.Common.UIComponents
{
    /// <summary>
    /// Represents the Text Area component that provides the editing capabilities for JSON document.
    /// </summary>
    public sealed class JsonTextAreaAttribute : UIComponentAttribute
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>JsonTextAreaAttribute</c> class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="label">The label of the component.</param>
        public JsonTextAreaAttribute(string name, string label)
            : base(name, label)
        {
        }

        #endregion Public Constructors
    }
}