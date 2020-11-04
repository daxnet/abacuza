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
        public Dictionary<string, string> AdditionalOptions { get; set; }
            = new Dictionary<string, string>();

        public IEnumerable<IEnumerable<KeyValuePair<string, object>>> ConfigurationUIElements
        {
            get
            {
                var result = new List<Dictionary<string, object>>();
                var uiComponentAttributes = from p in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            where p.CanRead && p.CanWrite && p.GetCustomAttributes().Any(p => p.GetType().IsSubclassOf(typeof(UIComponent)))
                                            let uiComponentAttribute = p.GetCustomAttributes().FirstOrDefault(attr => attr.GetType().IsSubclassOf(typeof(UIComponent)))
                                            let ordinal = uiComponentAttribute.GetType().GetProperty(nameof(UIComponent.Ordinal)).GetValue(uiComponentAttribute)
                                            let label = uiComponentAttribute.GetType().GetProperty(nameof(UIComponent.Label)).GetValue(uiComponentAttribute)
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

        #endregion Private Methods

        #region Public Methods

        public override string ToString() => Name;

        #endregion Public Methods
    }
}