using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SmartTickets.Models
{
    public class TicketsContext : DbContext
    {

        static TicketsContext()
        {
            Database.SetInitializer<TicketsContext>(null);
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}