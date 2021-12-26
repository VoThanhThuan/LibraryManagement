using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        [HttpGet("api/Statistical")]
        public async Task<IActionResult> GetData(DateTime? start = null, DateTime? end = null)
        {

            var statistical = await _statistical.GetStatiscals(start, end);

            return Ok(statistical);
        }

        public async Task<IActionResult> Export(DateTime? start = null, DateTime? end = null)
        {
            if (start != null && end != null)
            {
                if (start < end)
                {
                    TempData["error"] = "<Ngày bắt đầu> không thể nhỏ hơn <ngày kết thúc>";
                    ViewData["start"] = ((DateTime)start).ToString("yyyy-MM-dd");
                    ViewData["end"] = ((DateTime)start).AddDays(1).ToString("yyyy-MM-dd");

                } else
                {
                    ViewData["start"] = ((DateTime)start).ToString("yyyy-MM-dd");
                    ViewData["end"] = ((DateTime)end).ToString("yyyy-MM-dd");
                }
            }
            else
            {
                ViewData["start"] = DateTime.Now.ToString("yyyy-MM-dd");
                ViewData["end"] = DateTime.Now.ToString("yyyy-MM-dd");

            }

            var statistical = await _statistical.GetStatiscals(start, end);
            return View(statistical);
        }

    }
}
