using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.UI.Controllers
{
    public class BookInBorrowsController : Controller
    {
        // GET: BookInBorrowController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BookInBorrowController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookInBorrowController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookInBorrowController/Create
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

        // GET: BookInBorrowController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookInBorrowController/Edit/5
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

        // GET: BookInBorrowController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookInBorrowController/Delete/5
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
