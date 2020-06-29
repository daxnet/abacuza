// ==============================================================
//           _                               
//     /\   | |                              
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//                                           
// Data Processing Platform
// Copyright 2020 by daxnet. All rights reserved.
// Licensed under LGPL-v3
// ==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Abacuza.Common
{
    /// <summary>
    /// Represents that the implemented classes are entities that identifies themselves
    /// by their identifiers of <see cref="Guid"/> type.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; set; }
    }
}
