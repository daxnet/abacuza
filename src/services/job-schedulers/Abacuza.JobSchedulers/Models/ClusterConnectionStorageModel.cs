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

using Abacuza.Common;
using Abacuza.Common.DataAccess;
using Newtonsoft.Json;
using System;

namespace Abacuza.JobSchedulers.Models
{
    /// <summary>
    /// Represents the data model for a serialized Cluster Connection object.
    /// </summary>
    /// <seealso cref="Abacuza.Common.IEntity" />
    [StorageModel("ClusterConnections")]
    public class ClusterConnectionStorageModel : IEntity
    {

        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier of the data model.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the cluster connection.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="string"/> value which represents the settings
        /// of the cluster connection.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        [JsonProperty("settings")]
        public string Settings { get; set; }

        /// <summary>
        /// Gets or sets the name of the cluster which is this cluster connection
        /// used for.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty("type")]
        public string Type { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the string representation of this cluster connection.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Name;

        #endregion Public Methods
    }
}