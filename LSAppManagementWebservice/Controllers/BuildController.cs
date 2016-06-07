using LSAppManagementWebservice.Helpers;
using LSAppManagementWebservice.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LSAppManagementWebservice.Controllers
{
    public class BuildController : Controller
    {
        // POST: Build/Add
        //[HttpPost]
        public ActionResult Install(int ID, string SHA1, string DeployEnv)
        {
            string AppPath;

            try
            {
                using (var db = DbFactory.getAppDb())
                {
                    var app = (ApplicationModel)db.Applications.Find(ID);
                    if (app == null)
                    {
                        throw new Exception("App with id " + ID + " not found");
                    }

                    AppPath = @"C:/inetpub\DOTNET\" + app.Name;
                    
                    app.WebappPath = (DeployEnv == "Dev" ? "dev/" : "") + app.WebappPath;
                    AppBuildHelper.PullApp(app, SHA1);
                    AppBuildHelper.BuildApp(app, SHA1);

                    if (!IISHelper.AppExists("LS Web Site", app.Name))
                    {
                        IISHelper.AddApp("LS Web Site", app.Name, app.WebappPath, @"C:\inetpub\DOTNET\builds\" + app.Name + @"\");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Med("Pull and Build Failed:\n" + e.ToString() + "\n" + e.StackTrace);
                return Json(new {
                    msg = "failed",
                    error = e.ToString(),
                    stack = e.StackTrace
                }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                if (Directory.Exists(AppPath))
                {
                    Directory.Delete(AppPath, true);
                }
            }
            catch (Exception e)
            {
                Logger.Med("Error deleting pull directory:\n" + e.ToString() + "\n" + e.StackTrace);
            }

            Logger.Info("Pull and Build Success");
            return Json(new {
                msg = "success"
            }, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult Pull(int ID, string SHA1)
        {
            string AppPath;

            try
            {
                using (var db = DbFactory.getAppDb())
                {
                    var app = (ApplicationModel)db.Applications.Find(ID);
                    if (app == null)
                    {
                        throw new Exception("App with id " + ID + " not found");
                    }

                    AppPath = @"C:/inetpub\DOTNET\" + app.Name;

                    AppBuildHelper.PullApp(app, SHA1);
                }
            }
            catch (Exception e)
            {
                Logger.Med("Pull and Build Failed:\n" + e.ToString() + "\n" + e.StackTrace);
                return Json(new
                {
                    msg = "failed",
                    error = e.ToString(),
                    stack = e.StackTrace
                }, JsonRequestBehavior.AllowGet);
            }

            Logger.Info("Pull and Build Success");
            return Json(new
            {
                msg = "success"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Build(int ID, string SHA1)
        {
            string AppPath;

            try
            {
                using (var db = DbFactory.getAppDb())
                {
                    var app = (ApplicationModel)db.Applications.Find(ID);
                    if (app == null)
                    {
                        throw new Exception("App with id " + ID + " not found");
                    }

                    AppPath = @"C:/inetpub\DOTNET\" + app.Name;

                    AppBuildHelper.PullApp(app, SHA1);
                    AppBuildHelper.BuildApp(app, SHA1);
                }
            }
            catch (Exception e)
            {
                Logger.Med("Pull and Build Failed:\n" + e.ToString() + "\n" + e.StackTrace);
                return Json(new
                {
                    msg = "failed",
                    error = e.ToString(),
                    stack = e.StackTrace
                }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                if (Directory.Exists(AppPath))
                {
                    Directory.Delete(AppPath, true);
                }
            }
            catch (Exception e)
            {
                Logger.Med("Error deleting pull directory:\n" + e.ToString() + "\n" + e.StackTrace);
            }

            Logger.Info("Pull and Build Success");
            return Json(new
            {
                msg = "success"
            }, JsonRequestBehavior.AllowGet);
            
        }
    }
}