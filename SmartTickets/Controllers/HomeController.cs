using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SmartTickets.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SmartTickets.Controllers
{
    public class HomeController : Controller
    {
        TicketsContext db = new TicketsContext();

        /*public ActionResult Index()
        {
            return View();
        }*/

        //[Authorize]
        public ActionResult Index()
        {
            IList<string> roles = new List<string> { "Роль не определена" };
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindByEmail(User.Identity.Name);
            if (user != null)
                roles = userManager.GetRoles(user.Id);
            return View(roles);
        }
        [Authorize]
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
        [Authorize]
        public ActionResult ChangeTickets()
        {
            var email = User.Identity.GetUserName();
            var itemTickets = db.Orders.Where(x => x.Email == email).ToList();

            return View(itemTickets);
        }
        [Authorize]
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

        public ActionResult Search(string request)
        {

            if (string.IsNullOrEmpty(request))
                return RedirectToAction("BuyTickets");
            var result = db.Events.Include(b => b.Category).Where(x => x.Name.Contains(request)).ToList();
            result.AddRange(db.Events.Include(x => x.Category).Where(x => x.Artist.Contains(request)).ToList());
            result.AddRange(db.Events.Include(x => x.Category).Where(x => x.City.Contains(request)).ToList());
            result.AddRange(db.Events.Include(x => x.Category).Where(x => x.Place.Contains(request)).ToList());

            if (result.Count > 0)
            {
                ViewBag.Message = "Результаты поиска:";
            }
            else
            {
                ViewBag.Message = "Ничего не найдено";
            }

            return View(result);
        }
    }
}