using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LSAppManagementWebservice.Models.DB
{
    [Table("dbo.Applications")]
    public class ApplicationEntity
    {
        public static explicit operator ApplicationEntity(ApplicationModel app)
        {
            if (app == null)
            {
                return null;
            }
            return new ApplicationEntity()
            {
                Name = app.Name,
                Description = app.Description,
                ID = app.ID,
                WebappPath = app.WebappPath,
                GithubRepository = app.GithubRepository
            };
        }
        [Key]
        public int ID { get; set; }

        public bool Deleted { get; set; }
        [Column("ProjectName")]
        public string Name { get; set; }
        public string GithubRepository { get; set; }
        public string WebappPath { get; set; }
        public string Description { get; set; }
    }
}