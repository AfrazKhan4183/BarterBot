using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BarterBot2.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        public int SenRevID { get; set; }
        public int SenderID { get; set; }
       // public int ReceiverID { get; set; }
        public string Text { get; set; }
    }
}