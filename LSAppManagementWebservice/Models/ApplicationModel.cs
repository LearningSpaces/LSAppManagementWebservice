using LSAppManagementWebservice.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LSAppManagementWebservice.Models
{
    public class ApplicationModel
    {
        public Uri GithubAPI
        {
            get
            {
                return new Uri("https://api.github.com/repos/LearningSpaces/" + GithubRepository);
            }
        }

        public Uri GithubURL
        {
            get
            {
                return new Uri("http://github.com/LearningSpaces/" + GithubRepository);
            }
        }
        [Required]
        public string GithubRepository { get; set; }
        public string WebappPath { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public bool Deleted { get; set; }

        public static explicit operator ApplicationModel(ApplicationEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            ApplicationModel app = new ApplicationModel()
            {
                Name = entity.Name,
                Description = entity.Description,
                ID = entity.ID,
                WebappPath = entity.WebappPath,
                GithubRepository = entity.GithubRepository,
                Deleted = entity.Deleted
            };
            return app;
        }
    }
}