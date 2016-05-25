using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSAppManagementWebservice.Models.DB
{
    public class LogEntity
    {
        public int AppId { get; set; }
        public int Category { get; set; }
        public string Message { get; set; }
    }
}