﻿using System.Web.Mvc;
using $rootnamespace$.Models;
using $rootnamespace$.Repository;
using Thunder.Web.Mvc;
using Controller = System.Web.Mvc.Controller;

namespace $rootnamespace$.Controllers
{
    public class LayoutsController : Controller
    {
        public LayoutsController()
        {
            ModuleRepository = new ModuleRepository();
        }

        public IModuleRepository ModuleRepository { get; private set; }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView("Header");
        }

        [ChildActionOnly]
        [SessionPerRequest]
        public ActionResult Menu()
        {
            return PartialView("Menu", ModuleRepository.Find(new User { Id = XerneasAuthentication.GetUserData().Id }));
        }
    }
}