using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartTickets.Models;
using System.Data.Entity;
using System.Globalization;
using Microsoft.AspNet.Identity;


namespace SmartTickets.Controllers
{
    public class CartController : Controller
    {
        TicketsContext db = new TicketsContext();

        public ActionResult Add(int eventId)
        {
            var email = User.Identity.GetUserName();
            var items = db.ItemEvents.Where(x => x.EventId == eventId && x.Email == email).ToList();
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
                item.Email = email;
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

        public ActionResult Pay(int eventId)
        {
            var email = User.Identity.GetUserName();
            var list = db.Orders.Where(x => x.Email == email && x.EventId == eventId).ToList();
            int itemCount;
            ItemEvent itemEvent;
            if (list.Count() > 0)
            {
                var item = list.First();
                itemEvent = db.ItemEvents.First(x => x.EventId == eventId && x.Email == email);
                itemCount = itemEvent.Quantity;
                if (item.Count + itemCount > 5)
                {
                    return RedirectToAction("Error", "Cart", item);
                }
                item.Count += itemCount;
                item.Date = DateTime.Now;
                db.Entry(item).State = EntityState.Modified;
                db.Events.First(x => x.Id == eventId).Count -= itemCount;
                db.ItemEvents.Remove(itemEvent);
                db.SaveChanges();
                return View(item);
            }
            itemEvent = db.ItemEvents.First(x => x.EventId == eventId && x.Email == email);
            itemCount = itemEvent.Quantity;
            Order order = new Order();
            order.Email = email;
            order.EventId = eventId;
            order.Count = itemCount;
            order.Number = (email + (eventId*100).ToString() + itemCount.ToString()).GetHashCode().ToString();
            order.Date = DateTime.Now;
            db.Events.First(x => x.Id == eventId).Count -= itemCount;
            db.Orders.Add(order);
            db.ItemEvents.Remove(itemEvent);
            db.SaveChanges();
            return View();
        }

        public ActionResult Error(Order item)
        {
            
            return View(item);
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