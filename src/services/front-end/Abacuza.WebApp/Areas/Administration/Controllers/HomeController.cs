using Abacuza.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
            => _apiService = apiService;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageClusterConnections()
        {
            var clusterConnections = await _apiService.GetClusterConnectionsAsync();

            return View(clusterConnections);
        }

        public IActionResult ManageJobRunners()
        {
            return View();
        }

        public IActionResult ManageGroups()
        {
            return View();
        }

        public IActionResult ManageUsers()
        {
            return View();
        }

        public IActionResult InstalledPlugins()
        {
            return View();
        }
    }
}
