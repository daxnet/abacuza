using Abacuza.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.WebApp.ViewComponents
{
    [ViewComponent(Name = "RegisteredClusterTypesList")]
    public class RegisteredClusterTypesList : ViewComponent
    {
        private readonly ApiService _apiService;

        public RegisteredClusterTypesList(ApiService apiService)
            => _apiService = apiService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _apiService.GetRegisteredClusterTypesAsync());
        }
    }
}
