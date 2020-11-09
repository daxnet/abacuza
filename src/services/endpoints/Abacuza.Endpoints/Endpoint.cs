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

using Abacuza.Common.UIComponents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Abacuza.Endpoints
{
    public abstract class Endpoint : IEndpoint
    {
        #region Public Properties

        [TextArea("additionalOptions", "Additional options", Ordinal = -1)]
        public string AdditionalOptions { get; set; }

        public IEnumerable<IEnumerable<KeyValuePair<string, object>>> ConfigurationUIElements
        {
            get
            {
                var result = new List<Dictionary<string, object>>();
                var uiComponentAttributes = from p in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            where p.CanRead && p.CanWrite && p.GetCustomAttributes().Any(p => p.GetType().IsSubclassOf(typeof(UIComponentAttribute)))
                                            let uiComponentAttribute = p.GetCustomAttributes().FirstOrDefault(attr => attr.GetType().IsSubclassOf(typeof(UIComponentAttribute)))
                                            let ordinal = uiComponentAttribute.GetType().GetProperty(nameof(UIComponentAttribute.Ordinal)).GetValue(uiComponentAttribute)
                                            let label = uiComponentAttribute.GetType().GetProperty(nameof(UIComponentAttribute.Label)).GetValue(uiComponentAttribute)
                                            orderby ordinal descending, label ascending
                                            select new { EndpointPropertyName = p.Name, UIComponentAttribute = uiComponentAttribute };


                foreach(var attr in uiComponentAttributes)
                {
                    var properties = new Dictionary<string, object>
                    {
                        { "_type", GetNormalizedAttributeName(attr.UIComponentAttribute.GetType()) },
                        { "_property", attr.EndpointPropertyName }
                    };

                    var options = from p in attr.UIComponentAttribute.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  where p.Name != "TypeId"
                                  select p;

                    foreach (var option in options)
                    {
                        var optionValue = option.GetValue(attr.UIComponentAttribute);
                        properties.Add(option.Name, optionValue);
                    }

                    result.Add(properties);             
                }

                return result;
            }
        }

        public string Description => EndpointAttribute?.Description;
        public string DisplayName => EndpointAttribute?.DisplayName;
        public string Name => EndpointAttribute?.Name;
        public EndpointType Type => EndpointAttribute?.Type ?? EndpointType.None;
        #endregion Public Properties

        #region Private Properties

        private EndpointAttribute EndpointAttribute => this.GetType().IsDefined(typeof(EndpointAttribute), true) ?
                this.GetType().GetCustomAttribute<EndpointAttribute>() : null;

        #endregion Private Properties

        #region Private Methods

        private static string GetNormalizedAttributeName(Type attributeType)
        {
            return attributeType.Name.Remove(attributeType.Name.IndexOf("Attribute"));
        }

        public void ApplySettings(string settings)
        {
            var settingsArray = JArray.Parse(settings);
            foreach(var jobj in settingsArray)
            {
                var name = jobj["name"].ToObject<string>();
                var value = jobj["value"].ToObject<object>();
                var property = (from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                where p.CanWrite && p.GetCustomAttributes().Any(a => a.GetType().IsSubclassOf(typeof(UIComponentAttribute)))
                                let uiComponentAttribute = p.GetCustomAttributes().First(a => a.GetType().IsSubclassOf(typeof(UIComponentAttribute))) as UIComponentAttribute
                                where uiComponentAttribute.Name == name
                                select p).FirstOrDefault();

                if (property != null)
                {
                    if (value is JArray ja)
                    {
                        property.SetValue(this, ja.ToObject(property.PropertyType));
                    }
                    else
                    {
                        property.SetValue(this, value);
                    }
                }
            }
        }

        #endregion Private Methods

        #region Public Methods

        public override string ToString() => Name;

        #endregion Public Methods
    }
}