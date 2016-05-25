using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LSAppManagementWebservice.Models.DB
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(string conn)
            : base(conn)
        {
        }

        public virtual DbSet<ApplicationEntity> Applications { get; set; }
    }
}