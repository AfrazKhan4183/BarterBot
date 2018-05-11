using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BarterBot2.Models
{
    public class BarterBot2DbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Request> requests { get; set; }
        public DbSet<TimeBasedService> TimeBasedServices { get; set; }
        public DbSet<MoneyBasedService> MoneyBasedServices { get; set; }
        public DbSet<Interest> interests { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<Conversation> conversations { get; set; }
        public DbSet<Rank> ranks { get; set; }

        public System.Data.Entity.DbSet<BarterBot2.Models.Service> Services { get; set; }
    }
}