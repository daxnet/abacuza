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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Abacuza.Jobs.ApiService.Controllers
{
    /// <summary>
    /// Represents the controller that returns the health check results.
    /// </summary>
    [ApiController]
    [Route("api/healthcheck")]
    public class HealthCheckController : ControllerBase
    {
        #region Public Methods

        /// <summary>
        /// Retrieves the version of the current executing assembly.
        /// </summary>
        /// <returns>The version of the current executing assembly.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
            => Ok(new
            {
                version = Assembly.GetExecutingAssembly().GetName().Version!.ToString(),
                name = "job-service"
            });

        #endregion Public Methods
    }
}