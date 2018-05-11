using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarterBot2.Models
{
    public class TimeBasedService
    {
        [Key]
        public  int ID { get; set; }
        public int ServiceID { get; set; }


        [Required(ErrorMessage = "WorkTime is required")]
        public string WorkTime { get; set; }

        public string Detail { get; set; }
    }
}