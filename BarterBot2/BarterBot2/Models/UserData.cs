using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarterBot2.Models
{
    public class UserData
    {
        public  int ID;
        public List<Request> sentRequests = new List<Request>();
        public List<Request> comingRequests = new List<Request>();
        public List<Interest> interests = new List<Interest>();
        public List<Service> services = new List<Service>();
        public List<Service> offeredServices = new List<Service>();
        public List<Message> messages = new List<Message>();
        public List<int> conversationIds = new List<int>();
    }
}