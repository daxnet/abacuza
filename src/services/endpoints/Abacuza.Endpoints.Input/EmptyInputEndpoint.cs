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

namespace Abacuza.Endpoints.Input
{
    [Endpoint("endpoints.none.empty", 
        "Empty", 
        EndpointType.Input, 
        Description = "The endpoint that doesn't read from any data source.")]
    public sealed class EmptyInputEndpoint : Endpoint
    {
    }
}