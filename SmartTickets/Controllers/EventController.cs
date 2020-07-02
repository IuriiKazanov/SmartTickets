using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartTickets.Models;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;

namespace SmartTickets.Controllers
{
    [Authorize(Roles = "manager")]
    public class EventController : Controller
    {
        private TicketsContext db = new TicketsContext();
        private const int Height = 200, Width = 150;
        // GET: Event
        public ActionResult Index(int? categoryId)
        {
        
            List<Event> eventList = db.Events.OrderBy(x => x.Id).ToList();
            if (categoryId != null && categoryId != 0)
            {
                var category = db.Categories.Find(categoryId);
                if (category != null)
                    eventList = category.Events.OrderBy(x => x.Id).ToList();
            }

            var categoriesList = db.Categories.ToList();
            categoriesList.Insert(0, new Category() { Id = 0, Name = "все" });
            return View(eventList);
        }

        // GET: /Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event _event = db.Events.Find(id);
            if (_event == null)
            {
                return HttpNotFound();
            }
            var categories = db.Categories.Where(g => g.Id == _event.CategoryId).ToList();
            _event.Category = categories[0];
            return View(_event);
        }

        // GET: /Book/Create
        public ActionResult Create()
        {
            var categoriesList = db.Categories.ToList();
            SelectList categories = new SelectList(categoriesList, "Id", "Name");
            ViewBag.list = categories;
            return View();
        }

        // POST: /Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Artist,Date,City,Place,Price,Count,CategoryId")] Event _event, HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                if (CheckByGraphicsFormat(fileName))
                {
                    SaveImage(upload, fileName);
                    _event.ImageUrl = fileName;
                }
                else _event.ImageUrl = "event.jpg";
            }
            if (!ModelState.IsValid) return View(_event);
            db.Events.Add(_event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void SaveImage(HttpPostedFileBase upload, string fileName)
        {
            var image = new Bitmap(upload.InputStream);
            var smallImg = ResizeImage(image, Width, Height);
            smallImg.Save(Server.MapPath("~/Images/" + fileName));
        }

        private bool CheckByGraphicsFormat(string fileName)
        {
            var ext = fileName.Substring(fileName.Length - 3);
            return string.Compare(ext, "png", StringComparison.Ordinal) == 0 ||
                   string.Compare(ext, "jpg", StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }



        // GET: /Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event _event = db.Events.Find(id);
            if (_event == null)
            {
                return HttpNotFound();
            }
            var categoriesList = db.Categories.ToList();
            SelectList categories = new SelectList(categoriesList, "Id", "Name");
            ViewBag.list = categories;
            var _categories = db.Categories.Where(g => g.Id == _event.CategoryId).ToList();
            _event.Category = _categories[0];
            return View(_event);
        }

        // POST: /Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Artist,Date,City,Place,Price,Count,CategoryId,ImageUrl")] Event _event, HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                if (CheckByGraphicsFormat(fileName))
                {
                    SaveImage(upload, fileName);
                    _event.ImageUrl = fileName;
                }
            }
            if (!ModelState.IsValid) return View(_event);
            db.Entry(_event).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _event = db.Events.Find(id);
            if (_event == null)
            {
                return HttpNotFound();
            }
            var _categories = db.Categories.Where(g => g.Id == _event.CategoryId).ToList();
            _event.Category = _categories[0];
            return View(_event);
        }

        // POST: /Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var _event = db.Events.Find(id);
            if (_event != null) db.Events.Remove(_event);
            /*
            var orders = db.Orders.Where(x => x.EventId == id).ToList();
            foreach (var order in orders)
            {
                db.Orders.Remove(order);
            }*/
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory([Bind(Include = "Id,Name")] Category category)
        {
            if (!ModelState.IsValid) return View(category);
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}