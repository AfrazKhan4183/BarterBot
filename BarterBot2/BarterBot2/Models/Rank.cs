using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarterBot2.Models
{
    public class Rank
    {
        [Key]
        public int RankID { get; set; }

        public int UserID { get; set; }
        //[ForeignKey("UserID")]
        //public virtual User Users { get; set; }

        public int NumOfOrders { get; set; }

        public string Status { get; set; }

    }
}