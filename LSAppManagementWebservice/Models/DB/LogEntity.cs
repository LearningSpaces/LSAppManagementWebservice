using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LSAppManagementWebservice.Models.DB
{
    [Table("dbo.Log")]
    public class LogEntity
    {
        [Column("fk_Applications_ID")]
        public int AppId { get; set; }
        public int ID { get; set; }
        public DateTime LogDateTime { get; set; }
        public int Category { get; set; }
        public string Message { get; set; }
    }
}