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

using Abacuza.Clusters.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Abacuza.Clusters.Spark
{
    public sealed class SparkClusterConnection : IClusterConnection
    {
        #region Private Fields

        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        #endregion Private Fields

        #region Public Constructors

        public SparkClusterConnection() => this.BaseUrl = string.Empty;

        #endregion Public Constructors

        #region Public Properties

        public string BaseUrl { get; set; }
        public string ClusterType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<KeyValuePair<string, object>> Properties => _properties;

        #endregion Public Properties

        #region Public Methods

        public void DeserializeConfiguration(string serializedConfiguration)
        {
            var configurationJObject = JObject.Parse(serializedConfiguration);
            this.BaseUrl = configurationJObject["baseUrl"]?.Value<string>() ?? string.Empty;
            var props = configurationJObject["properties"]?.ToObject<Dictionary<string, object>>();
            _properties.Clear();
            if (props != null)
            {
                foreach (var kvp in props)
                {
                    _properties.Add(kvp.Key, kvp.Value);
                }
            }
        }

        public string SerializeConfiguration()
        {
            var serializingObject = new
            {
                baseUrl = BaseUrl,
                properties = _properties
            };

            return JsonConvert.SerializeObject(serializingObject);
        }

        public override string ToString() => Name ?? base.ToString();

        #endregion Public Methods
    }
}