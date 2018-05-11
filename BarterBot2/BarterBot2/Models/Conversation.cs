using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BarterBot2.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationID { get; set; }
        public int SenRevID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public int TotalMessages { get; set; }

    }
}