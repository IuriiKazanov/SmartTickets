using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BuyTickets()
        {
            return View();
        }

        public ActionResult ChangeTickets()
        {
            ViewBag.Message = "Change tickets";

            return View();
        }

        public ActionResult MyTickets()
        {
            ViewBag.Message = "Your tickets";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}