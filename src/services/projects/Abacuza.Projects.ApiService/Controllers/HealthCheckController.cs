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

using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Abacuza.Projects.ApiService.Controllers
{
    /// <summary>
    /// Represents the controller that returns the health check status.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/healthcheck")]
    public class HealthCheckController : ControllerBase
    {
        #region Public Methods

        [HttpGet]
        public IActionResult Get()
            => Ok(new
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            });

        #endregion Public Methods
    }
}