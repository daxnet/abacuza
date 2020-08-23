using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Controllers
{
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("healthcheck")]
        public IActionResult Get()
            => Ok(new
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            });
    }
}
