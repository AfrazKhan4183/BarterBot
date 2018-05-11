using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarterBot2.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }

        //[ForeignKey("UserID")]
        public int UserID { get; set; }
       //[ForeignKey("UserID")]
       // public virtual User Users { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
    }
}