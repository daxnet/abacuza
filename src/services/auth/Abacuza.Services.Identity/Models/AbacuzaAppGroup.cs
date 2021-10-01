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

using System;

namespace Abacuza.Services.Identity.Models
{
    /// <summary>
    /// Represents a user group in Abacuza.
    /// </summary>
    public class AbacuzaAppGroup
    {
        public AbacuzaAppGroup()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public AbacuzaAppGroup(string groupName) : this() => Name = groupName;

        public virtual string Id { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the user group.
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the description of the user group.
        /// </summary>
        public virtual string Description { get; set; }

        public override string ToString() => this.Name;
    }
}