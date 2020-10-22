using System;

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
