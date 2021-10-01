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

namespace Abacuza.Services.Identity.Models
{
    /// <summary>
    /// Represents the relationship between user and group.
    /// </summary>
    public class AbacuzaAppUserGroup
    {
        public virtual string UserId { get; set; }
        
        public virtual string GroupId { get; set; }
    }
}