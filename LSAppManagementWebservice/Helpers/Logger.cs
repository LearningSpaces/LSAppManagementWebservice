using LSAppManagement.Models;
using LSAppManagementWebservice.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace LSAppManagementWebservice.Helpers
{
    public static class Logger
    {
        public static int AppId
        {
            get
            {
                return Settings.AppId;
            }
        }

        public enum Category
        {
            Low,
            Med,
            High,
            Info
        }

        public static void Log(string msg, Category cat)
        {
            try
            {
                int CatNum;
                switch (cat)
                {
                    case Category.Info:
                        CatNum = 0;
                        break;
                    case Category.Low:
                        CatNum = 1;
                        break;
                    case Category.Med:
                        CatNum = 2;
                        break;
                    case Category.High:
                        CatNum = 3;
                        break;
                    default:
                        CatNum = -1;
                        break;
                }
                using (var db = DbFactory.getLogDb())
                {
                    db.Logs.Add(new LogEntity()
                    {
                        AppId = AppId,
                        Category = CatNum,
                        Message = msg,
                        LogDateTime = DateTime.Now
                    });
                    var ret = db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //We tried
                //Insert Code to Log using Logger ID
            }
        }

        public static void Low(string msg)
        {
            Log(msg, Category.Low);
        }

        public static void Med(string msg)
        {
            Log(msg, Category.Med);
        }

        public static void High(string msg)
        {
            Log(msg, Category.High);
        }

        public static void Info(string msg)
        {
            Log(msg, Category.Info);
        }
    }
}