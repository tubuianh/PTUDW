using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDevelop.Areas.Areas.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Areas/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}