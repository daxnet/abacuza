using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Controllers
{
    /// <summary>
    /// Represents the controller that returns the health check status.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("api/healthcheck")]
        public IActionResult Get()
            => Ok(new
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            });
    }
}
