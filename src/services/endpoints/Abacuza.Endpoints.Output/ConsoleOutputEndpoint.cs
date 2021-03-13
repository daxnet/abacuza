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

namespace Abacuza.Endpoints.Output
{
    /// <summary>
    /// Represents the output endpoint that writes the output to the console.
    /// </summary>
    [Endpoint("endpoint.output.console", "Console", EndpointType.Output)]
    public class ConsoleOutputEndpoint : Endpoint, IOutputEndpoint
    {
    }
}