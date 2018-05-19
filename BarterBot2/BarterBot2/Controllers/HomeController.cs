using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BarterBot2.Controllers
{
    public class HomeController : Controller
    {
        public static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static readonly List<Socket> clientSockets = new List<Socket>();
        public const int BUFFER_SIZE = 2048;
        public const int PORT = 100;
        public static readonly byte[] buffer = new byte[BUFFER_SIZE];

        public static int g = 0;

        public static List<string> data = new List<string>();
        public static List<string> data2 = new List<string>();
        public static List<string> data3 = new List<string>();

        public ActionResult Index()
        {
            if (g == 0)
            {
                SetupServer();
                g++;
            }


            return View();
        }

        public static void SetupServer()
        {    serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public static void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            serverSocket.Close();
        }

        public static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;
            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) 
            {
                return;
            }
            clientSockets.Add(socket);
            int length = clientSockets.Count;
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;

            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                current.Close();
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            

            MessagesController.IP = current.RemoteEndPoint.ToString();
            data.Add(AccountController.NAMEIP+"("+current.RemoteEndPoint.ToString()+ ") :");
            data2.Add("  \" "  +text +" \" ");
   
            
            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public  ActionResult Report()
        {
            return View("Report");
        }
    }
}