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

namespace Abacuza.Endpoints
{
    /// <summary>
    /// Represents the type of the endpoint.
    /// </summary>
    public enum EndpointType
    {
        /// <summary>
        /// Indicates that the endpoint is neither an Input nor an Output endpoint.
        /// </summary>
        None,

        /// <summary>
        /// Indicates an Input endpoint.
        /// </summary>
        Input,

        /// <summary>
        /// Indicates an Output endpoint.
        /// </summary>
        Output
    }
}