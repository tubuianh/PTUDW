﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;

namespace WebDevelop.Controllers
{
    public class SiteController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();
            int sodong = db.Products.Count();
            ViewBag.sodong = sodong;
            return View();
        }
    }
}