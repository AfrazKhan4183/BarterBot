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
    public class MoneyBasedServicesController : Controller
    {
        private BarterBot2DbContext db = new BarterBot2DbContext();

        // GET: MoneyBasedServices
        public ActionResult Index()
        {
            return View(db.MoneyBasedServices.ToList());
        }

        // GET: MoneyBasedServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoneyBasedService moneyBasedService = db.MoneyBasedServices.Find(id);
            if (moneyBasedService == null)
            {
                return HttpNotFound();
            }
            return View(moneyBasedService);
        }

        // GET: MoneyBasedServices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MoneyBasedServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServiceID,WorkPrice,Detail,Currency")] MoneyBasedService moneyBasedService)
        {
            int F_KEY = Convert.ToInt16(Session["EnteredServiceID"]);
            moneyBasedService.ServiceID = F_KEY;
            if (ModelState.IsValid)
            {
                db.MoneyBasedServices.Add(moneyBasedService);
                db.SaveChanges();
            }

            return RedirectToAction("LoggedIn", "Account");
        }

        // GET: MoneyBasedServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoneyBasedService moneyBasedService = db.MoneyBasedServices.Find(id);
            if (moneyBasedService == null)
            {
                return HttpNotFound();
            }
            return View(moneyBasedService);
        }

        // POST: MoneyBasedServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServiceID,WorkPrice,Detail,Currency")] MoneyBasedService moneyBasedService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moneyBasedService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(moneyBasedService);
        }

        // GET: MoneyBasedServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoneyBasedService moneyBasedService = db.MoneyBasedServices.Find(id);
            if (moneyBasedService == null)
            {
                return HttpNotFound();
            }
            return View(moneyBasedService);
        }

        // POST: MoneyBasedServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MoneyBasedService moneyBasedService = db.MoneyBasedServices.Find(id);
            db.MoneyBasedServices.Remove(moneyBasedService);
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
