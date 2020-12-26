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
// Licensed under LGPL-v3
// ==============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Abacuza.Common.Utilities
{
    /// <summary>
    /// Provides extension methods for specific types.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Pluralizes a given word.
        /// </summary>
        /// <param name="word">The word to be pluralized.</param>
        /// <param name="inputIsKnownToBeSingular">Indicates whether the input is known to be singular.</param>
        /// <returns>The pluralized word.</returns>
        public static string Pluralize(this string word, bool inputIsKnownToBeSingular = true)
        {
            return Vocabularies.Default.Pluralize(word, inputIsKnownToBeSingular);
        }

        /// <summary>
        /// Converts an array of key value pair to an ExpandoObject.
        /// </summary>
        /// <param name="src">The array of key value pair to be converted.</param>
        /// <returns>The converted ExpandoObject.</returns>
        /// <remarks>Referenced implementation: https://theburningmonk.com/2011/05/idictionarystring-object-to-expandoobject-extension-method/</remarks>
        public static ExpandoObject ToExpando(this IEnumerable<KeyValuePair<string, object>> src)
        {
            var expando = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)expando;
            foreach (var kvp in src)
            {
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = ((IDictionary<string, object>)kvp.Value).ToExpando();
                    expandoDict.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = ((IDictionary<string, object>)item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDict.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDict.Add(kvp);
                }
            }

            return expando;
        }
    }
}
