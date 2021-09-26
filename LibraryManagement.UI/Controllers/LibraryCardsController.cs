using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.UI.Controllers
{
    public class LibraryCardsController : Controller
    {
        // GET: LibraryCardController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LibraryCardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibraryCardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibraryCardController/Create
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

        // GET: LibraryCardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibraryCardController/Edit/5
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

        // GET: LibraryCardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibraryCardController/Delete/5
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
