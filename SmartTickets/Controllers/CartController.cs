using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartTickets.Models;
using System.Data.Entity;


namespace SmartTickets.Controllers
{
    public class CartController : Controller
    {
        TicketsContext db = new TicketsContext();
        string GetCooki(string key)
        {
            if (HttpContext.Request.Cookies.AllKeys.Length > 0 &&
                HttpContext.Request.Cookies.AllKeys.Contains(key))
            {
                return HttpContext.Request.Cookies[key].Value;
            }
            return null;
        }

        public ActionResult Add(int eventId)
        {
            var items = db.ItemEvents.Where(x => x.EventId == eventId).ToList();
            ItemEvent item = new ItemEvent();
            if (items.Count() > 0)
            {
                item = items.First();
                db.Entry(item).State = EntityState.Modified;
            }
            else
            {
                item.EventId = eventId;
                item.Quantity = 1;
                db.ItemEvents.Add(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Cart", item);
        }


        // GET: Cart
        public ActionResult Index(ItemEvent item)
        {
            var _event = db.Events.First(x => x.Id == item.EventId);
            item.Event = _event;
            decimal sum = _event.Price * item.Quantity;
            ViewBag.Sum = sum;
            return View(item);
        }

        public ActionResult Pay(string email, int count, int? eventId)
        {
            if (eventId != null)
            {
                Event item = db.Events.First(x => x.Id == eventId);

                return View();
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ChangeItemQuantity(int eventId, int newQuantity)
         {
            var item = db.ItemEvents.First(x => x.EventId == eventId);
            var _event = db.Events.First(x => x.Id == eventId);
            var delta = (newQuantity - item.Quantity) * _event.Price;

            item.Quantity = newQuantity;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return Json(delta);
        }
    }
}