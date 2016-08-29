using AKarbowski.Infrastructure.Managers;
using AKarbowski.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AKarbowski.Controllers
{
    public class CarsController : Controller
    {
        private CarManager _manager = new CarManager();
        // GET: Cars
        public ActionResult Index()
        {
            var result = _manager.GetCars();
            return View(result);
        }

        // GET: Cars/Details/5
        public ActionResult Details(int id)
        {
            var model = _manager.GetCarById(id);
            return View(model);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        public ActionResult Create(CarAddViewModel viewModel, IEnumerable<HttpPostedFileBase> thumb, IEnumerable<HttpPostedFileBase> files)
        {
            var result = _manager.AddCar(viewModel, thumb.FirstOrDefault(), files);
            switch (result)
            {
                case Infrastructure.Constans.ResultTypes.Ok:
                    return RedirectToAction("Index");

                case Infrastructure.Constans.ResultTypes.Failed:
                    return RedirectToAction("Index");

                default:
                    return RedirectToAction("Index");
            }
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _manager.GetCarById(id);
            return View(model);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int id)
        {
            ViewBag.CarId = id;
            return View();
        }

        // POST: Cars/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _manager.RemoveCar(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
