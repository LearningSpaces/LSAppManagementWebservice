using LSAppManagementWebservice.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSAppManagement.Models
{
    public class Settings
    {
        public static int AppId = 3;
        public static string AppConnectionString = @"Data Source=JACOB-LAPTOP\SQLEXPRESS;Initial Catalog=LearningSpaces;Integrated Security=False;User ID=sa;Password=password123;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
        public static string LogConnectionString = @"Data Source=JACOB-LAPTOP\SQLEXPRESS;Initial Catalog=LearningSpaces;Integrated Security=False;User ID=sa;Password=password123;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
    }
}