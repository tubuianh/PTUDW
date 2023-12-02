using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
namespace WebDevelop.Controllers
{
    public class ModuleController : Controller
    {
        // GET: Module
        MenusDAO menusDAO = new MenusDAO();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MainMenu()
        {
            List<Menus> list = menusDAO.getListByParentId(0);
            return View(list);
        }
    }
}