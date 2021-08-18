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

using Abacuza.Common.UIComponents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Abacuza.Endpoints
{
    /// <summary>
    /// Represents the base class for endpoints.
    /// </summary>
    /// <seealso cref="Abacuza.Endpoints.IEndpoint" />
    public abstract class Endpoint : IEndpoint
    {

        #region Public Properties

        /// <summary>
        /// Gets or sets the additional options.
        /// </summary>
        /// <value>
        /// The additional options.
        /// </value>
        [TextArea("additionalOptions", "Additional options", Ordinal = -1)]
        public string? AdditionalOptions { get; set; }

        /// <summary>
        /// Gets the configuration UI elements that provide the UI capabilities for users
        /// to configure the endpoint.
        /// </summary>
        /// <value>
        /// The configuration UI elements.
        /// </value>
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
                                            select new { EndpointPropertyName = p.Name, EndpointPropertyType = p.PropertyType, UIComponentAttribute = uiComponentAttribute };


                foreach (var attr in uiComponentAttributes)
                {
                    var properties = new Dictionary<string, object>
                    {
                        { "_type", GetNormalizedAttributeName(attr.UIComponentAttribute.GetType()) },
                        { "_property", attr.EndpointPropertyName },
                        { "_endpoint", this.Name }
                    };

                    var options = from p in attr.UIComponentAttribute.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  where p.Name != "TypeId"
                                  select p;

                    foreach (var option in options)
                    {
                        var optionValue = option.GetValue(attr.UIComponentAttribute);
                        properties.Add(option.Name, optionValue);
                    }

                    if (properties.ContainsKey("DefaultValue") && !string.IsNullOrEmpty(properties["DefaultValue"]?.ToString()))
                    {
                        var objValue = ConvertStringValueToObject(properties["DefaultValue"]?.ToString(), attr.EndpointPropertyType);
                        if (objValue != null)
                        {
                            properties.Add("DefaultValueObject", objValue);
                        }
                    }

                    result.Add(properties);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the description of the endpoint.
        /// </summary>
        /// <value>
        /// The description of the endpoint.
        /// </value>
        public string? Description => EndpointAttribute?.Description;

        /// <summary>
        /// Gets the display name of the endpoint.
        /// </summary>
        /// <value>
        /// The display name of the endpoint.
        /// </value>
        public string DisplayName => EndpointAttribute?.DisplayName ?? throw new ArgumentException("DisplayName is not defined on the EndpointAttribute decoration.");

        /// <summary>
        /// Gets the name of the endpoint.
        /// </summary>
        /// <value>
        /// The name of the endpoint.
        /// </value>
        public string Name => EndpointAttribute?.Name ?? throw new ArgumentException("Name is not defined on the EndpointAttribute decoration.");

        /// <summary>
        /// Gets the endpoint type.
        /// </summary>
        /// <value>
        /// The type of the endpoint.
        /// </value>
        public EndpointType Type => EndpointAttribute?.Type ?? EndpointType.None;

        #endregion Public Properties

        #region Private Properties

        private EndpointAttribute? EndpointAttribute => this.GetType().IsDefined(typeof(EndpointAttribute), true) ?
                this.GetType().GetCustomAttribute<EndpointAttribute>() : null;

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// Applies the specified JSON setting value to the current endpoint.
        /// </summary>
        /// <param name="settings">The JSON setting to be applied to the current endpoint.</param>
        public void ApplySettings(string settings)
        {
            // firstly apply the default values that are defined on the property attribute.
            var propertyWithDefaultValue = from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                           where p.CanWrite && p.GetCustomAttributes().Any(a => a.GetType().IsSubclassOf(typeof(UIComponentAttribute)))
                                           let uiComponentAttribute = p.GetCustomAttributes().First(a => a.GetType().IsSubclassOf(typeof(UIComponentAttribute))) as UIComponentAttribute
                                           where !string.IsNullOrEmpty(uiComponentAttribute.DefaultValue)
                                           select new { Property = p, DefaultValue = ConvertStringValueToObject(uiComponentAttribute.DefaultValue, p.PropertyType) };
            foreach (var p in propertyWithDefaultValue)
            {
                p.Property.SetValue(this, p.DefaultValue);
            }

            // then read in the settings configured from the project.
            var settingsArray = JArray.Parse(settings);
            foreach (var jobj in settingsArray)
            {
                var componentId = jobj["component"]!.ToObject<string>()!;
                var name = componentId.Substring(componentId.LastIndexOf('.') + 1, componentId.Length - componentId.LastIndexOf('.') - 1);
                var value = jobj["value"]!.ToObject<object>();
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

        public override string ToString() => Name;

        #endregion Public Methods

        #region Private Methods

        private static object? ConvertStringValueToObject(string? stringValue, Type type) => type.Name switch
        {
            "Int32" => Convert.ToInt32(stringValue),
            "Int16" => Convert.ToInt16(stringValue),
            "Int64" => Convert.ToInt64(stringValue),
            "Boolean" => Convert.ToBoolean(stringValue),
            "Single" => Convert.ToSingle(stringValue),
            "Double" => Convert.ToDouble(stringValue),
            "String" => stringValue,
            _ => stringValue
        };

        private static string GetNormalizedAttributeName(Type attributeType)
        {
            return attributeType.Name.Remove(attributeType.Name.IndexOf("Attribute"));
        }

        #endregion Private Methods

    }
}