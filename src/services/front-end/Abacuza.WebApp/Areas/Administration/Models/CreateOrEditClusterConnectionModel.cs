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

using Abacuza.WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Abacuza.WebApp.Areas.Administration.Models
{
    /// <summary>
    /// Represents the view model for creating or editing cluster connections.
    /// </summary>
    public class CreateOrEditClusterConnectionModel : ClusterConnection
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the available cluster types.
        /// </summary>
        public List<SelectListItem> AvailableClusterTypes { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Gets or sets a JSON string that represents the available cluster types.
        /// </summary>
        public string AvailableClusterTypesValue
        {
            get
            {
                // return string.Join('|', AvailableClusterTypes.Select(item => $"{item.Text}:{item.Value}"));
                return JsonConvert.SerializeObject(AvailableClusterTypes);
            }

            set
            {
                AvailableClusterTypes.Clear();
                AvailableClusterTypes.AddRange(JsonConvert.DeserializeObject<List<SelectListItem>>(value));
                //AvailableClusterTypes.Clear();
                //var entires = value.Split('|');
                //foreach (var ent in entires)
                //{
                //    var val = ent.Split(':');
                //    AvailableClusterTypes.Add(new SelectListItem(val[0], val[1]));
                //}
            }
        }

        #endregion Public Properties
    }
}