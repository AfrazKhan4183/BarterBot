using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarterBot2.Models
{
    public class MoneyBasedService
    {
        [Key]
        public int ID { get; set; }
        public int ServiceID { get; set; }
      //  [ForeignKey("ServiceID")]
       // public virtual Service Services { get; set; }

        [Required(ErrorMessage = "WorkPrice is required")]
        public int WorkPrice { get; set; }

        public string Detail { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        
        public string Currency { get; set; }
    }
}