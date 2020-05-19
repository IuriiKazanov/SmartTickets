using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SmartTickets.Models;
using Microsoft.AspNet.Identity;

namespace SmartTickets.Controllers
{
    public class HomeController : Controller
    {
        TicketsContext db = new TicketsContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BuyTickets(int? categoryId)
        {
            var categoriesList = db.Categories.ToList();
            categoriesList.Insert(0, new Category() { Id = 0, Name = "Все" });
            SelectList categories = new SelectList(categoriesList, "Id", "Name");
            ViewBag.Categories = categories;

            var events = db.Events.Include(b => b.Category);
            if (!(categoryId == null || categoryId == 0))
            {
                events = events.Where(b => b.CategoryId == categoryId);
            }

            var list = events.ToList();

            return View(list);
        }

        public ActionResult ChangeTickets()
        {
            var email = User.Identity.GetUserName();
            var itemTickets = db.Orders.Where(x => x.Email == email).ToList();

            return View(itemTickets);
        }

        public ActionResult MyTickets()
        {
            var email = User.Identity.GetUserName();
            var itemTickets = db.Orders.Where(x => x.Email == email).ToList();

            return View(itemTickets);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}