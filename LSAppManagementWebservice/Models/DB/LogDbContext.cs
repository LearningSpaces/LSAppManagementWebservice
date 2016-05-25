using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LSAppManagementWebservice.Models.DB
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(string conn) : base(conn) { }

        public virtual DbSet<LogEntity> Logs { get; set; }
    }
}