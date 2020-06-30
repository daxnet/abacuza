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

namespace Abacuza.JobSchedulers.Common
{
    public abstract class ClusterConnection : IClusterConnection
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ClusterConnection connection &&
                   Id.Equals(connection.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public override string ToString() => Name;
    }
}