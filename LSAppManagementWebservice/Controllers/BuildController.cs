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
            try
            {
                using (var db = DbFactory.getAppDb())
                {
                    var app = (ApplicationModel)db.Applications.Find(ID);
                    if (app == null)
                    {
                        throw new Exception("App with id " + ID + " not found");
                    }

                    string AppPath = @"C:/inetpub\DOTNET\" + app.Name;

                    if (Directory.Exists(AppPath))
                    {
                        if (Directory.Exists(AppPath + "_Old"))
                        {
                            Directory.Delete(AppPath + "_Old", true);
                        }

                        Directory.Move(AppPath, AppPath + "_Old");
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
                        
                        if (Proc.ExitCode > 1) {
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
                        if (Proc.ExitCode > 1)
                        {
                            throw new Exception(error);
                        }
                        Logger.Info(output);
                    }

                    startInfo.FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";
                    startInfo.Arguments = "\"" + AppPath + @"\" + app.Name + ".sln\"" + @" /t:Build /p:Configuration=Release /p:TargetFramework=v4.0";

                    using (Process Proc = Process.Start(startInfo))
                    {
                        var output = Proc.StandardOutput.ReadToEnd();
                        var error = Proc.StandardError.ReadToEnd();
                        Proc.WaitForExit();
                        if (Proc.ExitCode > 1)
                        {
                            throw new Exception(error);
                        }
                        Logger.Info(output);
                    }

                    if (Directory.Exists(AppPath + "_Old"))
                    {
                        Directory.Delete(AppPath + "_Old", true);
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