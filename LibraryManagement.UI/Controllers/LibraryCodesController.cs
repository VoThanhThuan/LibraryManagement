using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.UI.Controllers
{
    public class LibraryCodesController : Controller
    {
        // GET: LibraryCodeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LibraryCodeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibraryCodeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibraryCodeController/Create
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

        // GET: LibraryCodeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibraryCodeController/Edit/5
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

        // GET: LibraryCodeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibraryCodeController/Delete/5
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
