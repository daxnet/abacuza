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

namespace Abacuza.Common
{
    /// <summary>
    /// Represents that the implemented classes are the collection
    /// that contains a specific page of the records along with
    /// the pagination information.
    /// </summary>
    public interface IPagedResult
    {
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of total records.
        /// </summary>
        /// <value>
        /// The number of total records.
        /// </value>
        long TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        long TotalPages { get; set; }
    }
}