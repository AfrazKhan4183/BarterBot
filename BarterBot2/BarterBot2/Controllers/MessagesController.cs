using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BarterBot2.Models;
using System.Net.Sockets;
using System.Text;

namespace BarterBot2.Controllers
{
    public class MessagesController : Controller
    {
        private BarterBot2DbContext db = new BarterBot2DbContext();
        private static Conversation c;
        private  static int reverseId;


        private static Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 100;
        private  static string REQUEST;
        public static string IP;

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
            REQUEST = message.Text;

            if(ModelState.IsValid)
            {
                db.messages.Add(message);
                db.SaveChanges();
                RequestLoop();
            }

            return RedirectToAction("CreateConversation");
        }

        public ActionResult Conversation(int Rid)
        {
            
            BarterBot2.Models.User r = db.users.Single(d => d.UserID == Rid);
            HomeController.data3.Add(r.FirstName);

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

        public static void ConnectToServer()
        {
            ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            }
        }

        private  void RequestLoop()
        {
            
                SendRequest();
         //       ReceiveResponse();
            
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private  void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            
            Environment.Exit(0);
        }

        private  void SendRequest()
        {
            string request = REQUEST;
            SendString(request);
            if (request.ToLower() == "exit")
            {
                Exit();
            }
        }

        private  void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private  void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Console.WriteLine(text);
        }
    }

}

