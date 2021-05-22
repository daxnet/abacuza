using Abacuza.WebApp.Areas.Administration.Models;
using Abacuza.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class ClusterConnectionsController : Controller
    {
        private readonly ApiService _apiService;

        public ClusterConnectionsController(ApiService apiService)
            => _apiService = apiService;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateOrEdit()
        {
            var registeredClusterTypes = await _apiService.GetRegisteredClusterTypesAsync();
            var model = new CreateOrEditClusterConnectionModel();
            
            if (registeredClusterTypes != null)
            {
                var items = registeredClusterTypes.Select(r => new SelectListItem(r.Name, r.Type))!;
                model.AvailableClusterTypes.AddRange(items);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(CreateOrEditClusterConnectionModel model)
        {
            try
            {
                if (model.Id == null)
                {
                    await _apiService.CreateClusterConnectionAsync(model);
                    return RedirectToAction(nameof(HomeController.ManageClusterConnections), "Home");
                }
            }
            catch (Exception ex)
            {
                this.ShowToastMessage(ex.ToString(), "Error", type: Utils.ToastMessageType.Error);
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] dynamic model)
        {
            var id = (string)model.id;
            try
            {
                await _apiService.DeleteClusterConnectionAsync(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
