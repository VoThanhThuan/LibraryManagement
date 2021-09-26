﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.UI.Controllers
{
    public class BookInGenresController : Controller
    {
        // GET: BookInGenreController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BookInGenreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookInGenreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookInGenreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookInGenreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookInGenreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookInGenreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookInGenreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
