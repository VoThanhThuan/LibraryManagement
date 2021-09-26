using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.UI.Controllers
{
    public class BorrowsController : Controller
    {
        // GET: BorrowController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BorrowController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BorrowController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BorrowController/Create
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

        // GET: BorrowController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BorrowController/Edit/5
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

        // GET: BorrowController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BorrowController/Delete/5
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
