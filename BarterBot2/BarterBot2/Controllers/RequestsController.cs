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
    public class RequestsController : Controller
    {
        private BarterBot2DbContext db = new BarterBot2DbContext();

        // GET: Requests
        public ActionResult Index()
        {
            return View(db.requests.ToList());
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        //[ValidateAntiForgeryToken]
        public ActionResult Create(int PSid,int Pid,int Sid)
        {
            BarterBot2DbContext db = new BarterBot2DbContext();
            Request request = new Models.Request();
            request.ProviderServiceID = PSid;
            request.ProviderUserID = Pid;
           // int i = db.requests.Count();
            request.SeekerUserID = Sid;
            //request.RequestID = i + 1;
            request.Status = "sent";

            if (ModelState.IsValid)
            {
                db.requests.Add(request);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return RedirectToAction("LoggedIn", "Account");
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestID,ProviderUserID,SeekerUserID,ProviderServiceID,Status")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Requests/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Request request = db.requests.Find(id);
        //    if (request == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(request);
        //}

        // POST: Requests/Delete/5
       // [HttpPost, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public ActionResult Delete(int Rid)
        {
            Request request = db.requests.Find(Rid);
            db.requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("LoggedIn","Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Reject(int Rid)
        {
            BarterBot2DbContext db = new BarterBot2DbContext();
            foreach(Request r in db.requests)
            {
                if(r.RequestID == Rid)
                {
                    r.Status = "Rejected";
                
                }
            }
            db.SaveChanges();

            return RedirectToAction("LoggedIn", "Account");
        }

        public ActionResult Accept(int Rid)
        {
            foreach (Request r in db.requests)
            {
                if (r.RequestID == Rid)
                {
                    r.Status = "Accepted";
                    
                    Rank Prov = db.ranks.Single(s => s.UserID == r.ProviderUserID);
                    Rank Seeker = db.ranks.Single(s => s.UserID == r.SeekerUserID);

                    db.ranks.Find(Prov.RankID).NumOfOrders++;
                    db.ranks.Find(Seeker.RankID).NumOfOrders++;
                    if(db.ranks.Find(Prov.RankID).NumOfOrders >2)
                    {
                        db.ranks.Find(Prov.RankID).Status = "Talented";
                    }else if(db.ranks.Find(Prov.RankID).NumOfOrders > 4)
                    {
                        db.ranks.Find(Prov.RankID).Status = "Proficient";
                    }else if(db.ranks.Find(Prov.RankID).NumOfOrders> 6)
                    {
                        db.ranks.Find(Prov.RankID).Status = "Experienced";
                    }else if(db.ranks.Find(Prov.RankID).NumOfOrders > 8)
                    {
                        db.ranks.Find(Prov.RankID).Status = "Expert";
                    }

                    if (db.ranks.Find(Seeker.RankID).NumOfOrders > 2)
                    {
                        db.ranks.Find(Seeker.RankID).Status = "Talented";
                    }
                    else if (db.ranks.Find(Seeker.RankID).NumOfOrders > 4)
                    {
                        db.ranks.Find(Seeker.RankID).Status = "Proficient";
                    }
                    else if (db.ranks.Find(Seeker.RankID).NumOfOrders > 6)
                    {
                        db.ranks.Find(Seeker.RankID).Status = "Experienced";
                    }
                    else if (db.ranks.Find(Seeker.RankID).NumOfOrders > 8)
                    {
                        db.ranks.Find(Seeker.RankID).Status = "Expert";
                    }

                }
            }
            db.SaveChanges();


            return RedirectToAction("LoggedIn", "Account");
        }
    }
}
