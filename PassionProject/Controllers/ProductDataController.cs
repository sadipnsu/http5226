﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassionProject.Controllers
{
    public class ProductDataController : Controller
    {
        // GET: ProductData
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProductData/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductData/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductData/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductData/Edit/5
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

        // GET: ProductData/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductData/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
