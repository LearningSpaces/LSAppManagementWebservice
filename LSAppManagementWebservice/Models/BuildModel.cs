using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSAppManagementWebservice.Models
{
    public class BuildModel
    {
        public string Name { get; set; }
        public Uri GithubDir { get; set; }
        public string SHA1 { get; set; }
        public string webpath { get; set; }
        public string AppPoolName { get; set; }
    }
}