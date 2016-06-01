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
        public ActionResult Add(int ID, string SHA1)
        {
            string AppPath = "";

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

                    if (Directory.Exists(AppPath))
                    {
                        Directory.Delete(AppPath, true);
                    }

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "git.exe";
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.Arguments = "clone " + app.GithubURL.ToString() + " \"" + AppPath + "\"";

                    using (Process Proc = Process.Start(startInfo))
                    {
                        var output = Proc.StandardOutput.ReadToEnd();
                        var error = Proc.StandardError.ReadToEnd();
                        Proc.WaitForExit();
                        
                        if (Proc.ExitCode > 0) {
                            throw new Exception(error);
                        }
                        Logger.Info(output);
                    }

                    startInfo.WorkingDirectory = AppPath;
                    startInfo.Arguments = "checkout " + SHA1 + " .";

                    using (Process Proc = Process.Start(startInfo))
                    {
                        var output = Proc.StandardOutput.ReadToEnd();
                        var error = Proc.StandardError.ReadToEnd();
                        Proc.WaitForExit();
                        if (Proc.ExitCode > 0)
                        {
                            throw new Exception(error);
                        }
                        Logger.Info(output);
                    }

                    if (Directory.Exists(AppPath + @"\packages\"))
                    {
                        Directory.Delete(AppPath + @"\packages\", true);
                    }

                    if (System.IO.File.Exists(AppPath + @"\.nuget\Nuget.exe")) 
                    {
                        System.IO.File.Delete(AppPath + @"\.nuget\Nuget.exe");
                    }

                    startInfo.FileName = "msbuild";
                    startInfo.Arguments = "\"" + AppPath + @"\" + app.Name + ".sln\"" + @" /p:DeployOnBuild=true;PublishProfile=C:\inetpub\DOTNET\filesystem_publish.pubxml;Configuration=Release";

                    using (Process Proc = Process.Start(startInfo))
                    {
                        var output = Proc.StandardOutput.ReadToEnd();
                        var error = Proc.StandardError.ReadToEnd();
                        Proc.WaitForExit();
                        if (Proc.ExitCode > 0)
                        {
                            throw new Exception(error);
                        }
                        Logger.Info(output);
                    }

                    if (!IISHelper.AppExists("Default Web Site", app.Name))
                    {
                        IISHelper.AddApp("Default Web Site", app.Name, app.WebappPath, @"C:\inetpub\DOTNET\builds\" + app.Name + @"\");
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

        // POST: Build/Remove
        [HttpPost]
        public ActionResult Remove(int id)
        {
            return View();
        }
    }
}