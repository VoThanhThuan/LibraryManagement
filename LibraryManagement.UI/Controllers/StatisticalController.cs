using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.UI.Services;

namespace LibraryManagement.UI.Controllers
{
    public class StatisticalController : Controller
    {


        private readonly StatisticalService _statistical;

        public StatisticalController(StatisticalService statistical)
        {
            _statistical = statistical;
        }

        public async Task<IActionResult> Index()
        {

            var statistical = await _statistical.GetStatiscals();

            return View(statistical);
        }
    }
}
