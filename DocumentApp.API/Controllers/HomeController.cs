using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DocumentApp.API.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
