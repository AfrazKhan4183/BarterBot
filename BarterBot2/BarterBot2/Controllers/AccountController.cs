using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BarterBot2.Models;

namespace BarterBot2.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public static List<Request> SentRequests = new List<Models.Request>();
        public static List<Request> ComingRequests = new List<Models.Request>();
        public static List<Service> OfferedServices = new List<Service>();
        

        public ActionResult Index()
        {
            using (BarterBot2DbContext db = new BarterBot2DbContext())
            {
                return View(db.users.ToList());
            }
        }

        //Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            //user.Country = "USA";
            if (ModelState.IsValid)
            {
                using (BarterBot2DbContext db = new BarterBot2DbContext())
                {
                    Rank r = new Rank();
                    r.NumOfOrders = 0;
                    r.Status = "Bigginer";

                    db.users.Add(user);
                    
                    db.SaveChanges();
                    r.UserID = user.UserID;

                    db.ranks.Add(r);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = user.FirstName + "successfully registered!";
            }
            return View();
        }


        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (BarterBot2DbContext db = new BarterBot2DbContext())
            {
                var usr = db.users.SingleOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                if (usr != null)
                {
                    Session["UserId"] = usr.UserID.ToString();
                    Session["Email"] = usr.Email.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
            }
            return View();
        }


        //LoggedIn

        public ActionResult LoggedIn()
        {
            
            OfferedServices.Clear();

            BarterBot2DbContext db = new BarterBot2DbContext();

            List<Service> services = new List<Service>();
            List<Interest> interests = new List<Interest>();
            List<Request> requests = new List<Request>();

            if (Session["UserId"] != null)
            {
                foreach (Service s in db.Services)
                {
                    if (s.UserID == Convert.ToInt32(Session["UserId"]))
                    {
                        services.Add(s);
                    }
                }

                foreach (Interest i in db.interests)
                {
                    if (i.UserID == Convert.ToInt32(Session["UserId"]))
                    {
                        interests.Add(i);
                    }
                }



                UserData ud = new UserData();
                ud.interests = interests.ToList();
                ud.services = services.ToList();
                int ID = Convert.ToInt32(Session["UserId"]);


                if (db.requests.Count() != 0)
                {
                    foreach (Service s in db.Services)
                    {
                        if (s.UserID != ID)
                        {
                            OfferedServices.Add(s);
                        }
                    }

                    foreach(Request r in db.requests)
                    {
                        foreach(Service s in OfferedServices.ToList())
                        {
                            if(r.SeekerUserID == ID && s.ServiceID == r.ProviderServiceID)
                            {
                                OfferedServices.Remove(s);
                            }
                        }
                    }
                }
                else
                {
                    foreach (Service s in db.Services)
                    {
                        if (s.UserID != Convert.ToInt32(Session["UserId"]))
                        {
                            OfferedServices.Add(s);
                        }
                    }
                }


                foreach (Request r in db.requests)
                {
                    if (r.ProviderUserID == Convert.ToInt32(Session["UserId"]) && r.Status == "sent")
                    {
                        ud.comingRequests.Add(r);
                    }
                }

                foreach (Request r in db.requests)
                {
                    if (r.SeekerUserID == Convert.ToInt32(Session["UserId"]) )
                    {
                        ud.sentRequests.Add(r);
                    }
                }


               // ud.comingRequests = ComingRequests.ToList();
               // ud.sentRequests = SentRequests.ToList();
                ud.offeredServices = OfferedServices.ToList();

                List<int> conversationIDs = new List<int>();
                List<Message> Messages = new List<Message>();
                

                foreach (Conversation g in db.conversations)
                {
                    
                    if ( g.SenderID == ID || g.ReceiverID == ID  )
                    {
                        conversationIDs.Add(g.SenRevID);

                        string merge = g.ReceiverID.ToString()+ g.SenderID.ToString();
                        int reverse = Convert.ToInt32(merge);

                        conversationIDs.Add(reverse);
                    }
                    
                }

                foreach(Message m in db.messages)
                {
                    foreach(int Mid in conversationIDs)
                    {
                        if(m.SenRevID == Mid )
                        {
                            Messages.Add(m);
                        }
                    }
                }

                ud.messages = Messages;
                ud.ID = ID;

                Session["LoginFlag"] = 1;
                return View(ud);
                
            }

            else
            {
                Session["LoginFlag"] = null;
                return RedirectToAction("Login");
            }
        }

        //Logout

        public ActionResult Logout()
        {
            Session.Clear();
            Session["LoginFlag"] = null;
            return RedirectToAction("Login","Account");
        }
        
    }

}
