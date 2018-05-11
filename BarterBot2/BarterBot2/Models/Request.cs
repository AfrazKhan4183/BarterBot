using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarterBot2.Models
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }

        public int ProviderUserID { get; set; }
        //[ForeignKey("ProviderUserID")]

        public int SeekerUserID { get; set; }
        //[ForeignKey("SeekerUserID")]
        //public virtual User Users { get; set; }

        public int ProviderServiceID { get; set; }
        //[ForeignKey("ProviderServiceID")]
        //public virtual Service Services { get; set; }

        public string Status { get; set; }
    }
}