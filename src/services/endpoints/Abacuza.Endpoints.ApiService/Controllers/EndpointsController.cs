using Abacuza.Endpoints.ApiService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Endpoints.ApiService.Controllers
{
    [Route("api/endpoints")]
    [ApiController]
    public class EndpointsController : ControllerBase
    {
        private readonly EndpointCollection _endpoints;
        private readonly ILogger<EndpointsController> _logger;

        public EndpointsController(EndpointCollection endpoints,
            ILogger<EndpointsController> logger)
        {
            _endpoints = endpoints;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetEndpoints([FromQuery(Name = "type")] string endpointType)
        {
            EndpointType? type = endpointType?.ToLower() switch
            {
                "input" => EndpointType.Input,
                "output" => EndpointType.Output,
                "none" => EndpointType.None,
                _ => null
            };

            return Ok(_endpoints.GetEndpointsByType(type).Select(e =>
                new
                {
                    e.Name,
                    e.DisplayName,
                    e.Description,
                    e.Type,
                    TypeName = Enum.GetName(typeof(EndpointType), e.Type),
                    e.ConfigurationUIElements
                }));
        }
    }
}
