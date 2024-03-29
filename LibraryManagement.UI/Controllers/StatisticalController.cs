﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LibraryManagement.UI.Services;
using Library.Library.Entities.ViewModels;

namespace LibraryManagement.UI.Controllers
{
    public class StatisticalController : Controller
    {


        private readonly StatisticalService _statistical;

        public StatisticalController(StatisticalService statistical)
        {
            _statistical = statistical;
        }

        public async Task<IActionResult> Index(DateTime? start = null, DateTime? end = null)
        {
            var statistical = await GetStatiscals(start, end);

            return View(statistical);
        }

        [HttpGet("api/Statistical")]
        public async Task<IActionResult> GetData(DateTime? start = null, DateTime? end = null)
        {

            var statistical = await GetStatiscals(start, end);

            return Ok(statistical);
        }

        public async Task<IActionResult> Export(DateTime? start = null, DateTime? end = null)
        {

            var statistical = await GetStatiscals(start, end);

            return View(statistical);
        }


        private async Task<StatisticalVM> GetStatiscals(DateTime? start = null, DateTime? end = null)
        {
            if (start != null && end != null) {
                if (start < end) {
                    ViewData["start"] = ((DateTime)start).ToString("yyyy-MM-dd");
                    ViewData["end"] = ((DateTime)end).ToString("yyyy-MM-dd");

                } else {
                    TempData["error"] = "<Ngày bắt kết thúc> không thể nhỏ hơn <ngày bắt đầu>";
                    ViewData["start"] = ((DateTime)start).ToString("yyyy-MM-dd");
                    ViewData["end"] = ((DateTime)start).AddDays(1).ToString("yyyy-MM-dd");

                }
            } else {
                ViewData["start"] = DateTime.Now.ToString("yyyy-MM-dd");
                ViewData["end"] = DateTime.Now.ToString("yyyy-MM-dd");
                start = DateTime.Now; end = DateTime.Now;

            }

            var statistical = await _statistical.GetStatiscals(start, end);
            return statistical;
        }
    }
}
