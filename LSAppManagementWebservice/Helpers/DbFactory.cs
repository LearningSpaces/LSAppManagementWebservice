using LSAppManagement.Models;
using LSAppManagementWebservice.Models;
using LSAppManagementWebservice.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSAppManagementWebservice.Helpers
{
    public class DbFactory
    {
        public static ApplicationDbContext getAppDb()
        {
            return new ApplicationDbContext(Settings.AppConnectionString);
        }

        public static LogDbContext getLogDb()
        {
            return new LogDbContext(Settings.LogConnectionString);
        }
    }
}