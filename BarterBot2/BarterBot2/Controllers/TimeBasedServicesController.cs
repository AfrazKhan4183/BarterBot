using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BarterBot2.Models;

namespace BarterBot2.Controllers
{
    public class TimeBasedServicesController : Controller
    {
        private BarterBot2DbContext db = new BarterBot2DbContext();

        // GET: TimeBasedServices
        public ActionResult Index()
        {
            return View(db.TimeBasedServices.ToList());
        }

        // GET: TimeBasedServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeBasedService timeBasedService = db.TimeBasedServices.Find(id);
            if (timeBasedService == null)
            {
                return HttpNotFound();
            }
            return View(timeBasedService);
        }

        // GET: TimeBasedServices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimeBasedServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServiceID,WorkTime,Detail")] TimeBasedService timeBasedService)
        {
            int F_KEY = Convert.ToInt16(Session["EnteredServiceID"]);
            timeBasedService.WorkTime += "Hour"; 
            timeBasedService.ServiceID = F_KEY;
            if (ModelState.IsValid)
            {
                db.TimeBasedServices.Add(timeBasedService);
                db.SaveChanges();
            }

            return RedirectToAction("LoggedIn","Account");
        }

        // GET: TimeBasedServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeBasedService timeBasedService = db.TimeBasedServices.Find(id);
            if (timeBasedService == null)
            {
                return HttpNotFound();
            }
            return View(timeBasedService);
        }

        // POST: TimeBasedServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServiceID,WorkTime,Detail")] TimeBasedService timeBasedService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeBasedService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeBasedService);
        }

        // GET: TimeBasedServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeBasedService timeBasedService = db.TimeBasedServices.Find(id);
            if (timeBasedService == null)
            {
                return HttpNotFound();
            }
            return View(timeBasedService);
        }

        // POST: TimeBasedServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeBasedService timeBasedService = db.TimeBasedServices.Find(id);
            db.TimeBasedServices.Remove(timeBasedService);
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
