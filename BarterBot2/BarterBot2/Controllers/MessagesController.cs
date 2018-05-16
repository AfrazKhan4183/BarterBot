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
    public class MessagesController : Controller
    {
        private BarterBot2DbContext db = new BarterBot2DbContext();
        private static Conversation c;
        private  static int reverseId;
        // GET: Messages
        public ActionResult Index()
        {
            return View(db.messages.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //GET: Messages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageID,SenRevID,SenderID,Text")] Message message)
        {
          
            int i = db.messages.Count();
            message.MessageID = i + 1;
            message.SenRevID = c.SenRevID;
            message.SenderID = c.SenderID;
            
            if(ModelState.IsValid)
            {
                db.messages.Add(message);
                db.SaveChanges();
  
            }

            return RedirectToAction("CreateConversation");
        }

        public ActionResult Conversation(int Rid)
        {
            int sender = Convert.ToInt32(Session["UserId"]);
            
            Conversation c1 = new Models.Conversation();
            c1.SenderID = sender;
            c1.ReceiverID = Rid;
            string SR = sender.ToString() + Rid.ToString();
            string SR2 = Rid.ToString() + sender.ToString();
            reverseId = Convert.ToInt32(SR2) ;
            c1.SenRevID = Convert.ToInt32(SR);
            c = c1;
           
            return RedirectToAction("create");
        }

        public ActionResult CreateConversation()
        {
            BarterBot2DbContext db2 = new BarterBot2DbContext();
            Conversation cc = new Models.Conversation();
           // Conversation ccc = new Models.Conversation();
            //ccc = db2.conversations.Single(s => s.SenRevID == c.SenRevID);
            
            if(db2.conversations.Count()!=0)
            {
                
                int concount = 0;
                int concountr = 0;

                int concount1 = db2.conversations.Count();

                foreach(Conversation cC in db2.conversations)
                {
                    if(cC.SenRevID == c.SenRevID )
                    {
                        db2.conversations.Single(s => s.SenRevID == c.SenRevID).TotalMessages++;
                        
                    }
                    else
                    {
                        concount++;
                    }
                }

                foreach (Conversation cC in db2.conversations)
                {
                    if (cC.SenRevID == reverseId)
                    {
                        db2.conversations.Single(s => s.SenRevID == reverseId).TotalMessages++;
                      
                    }
                    else
                    {
                        concountr++;
                    }
                    
                }

                if (concount == concount1 && concountr == concount1)
                {
                    c.TotalMessages = 1;
                    cc = c;
                    db2.conversations.Add(cc);

                }

                db2.SaveChanges();
            }
            else{
                        c.TotalMessages = 1;
                        cc = c;
                        db2.conversations.Add(cc);
                        db2.SaveChanges();
                    }
            
            return RedirectToAction("LoggedIn", "Account");
        }


        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageID,SenRevID,SenderID,Text")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int SedRecid, int reverse)
        {
             
            foreach(Conversation c in db.conversations)
            {
                if(c.SenRevID == SedRecid || c.SenRevID == reverse)
                {
                    db.conversations.Remove(c);
                    
                }
            }

            foreach (Message m in db.messages )
            {
                if (m.SenRevID == SedRecid || m.SenRevID == reverse)
                {
                    db.messages.Remove(m);
                    
                }
            }
            db.SaveChanges();
            return RedirectToAction("LoggedIn", "Account");
        }

        // POST: Messages/Delete/5
     

       
    }
}
