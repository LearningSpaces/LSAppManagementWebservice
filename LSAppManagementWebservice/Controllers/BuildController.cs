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
    [Authorize]
    public class BuildController : Controller
    {
        // POST: Build/Add
        [HttpPost]
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

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "git.exe";
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;
                    startInfo.Arguments = "clone " + app.GithubURL.ToString() + @" C:/inetpub\DOTNET\" + app.Name;

                    using (Process Proc = Process.Start(startInfo))
                    {
                        var output = Proc.StandardOutput.ReadToEnd();
                        Proc.WaitForExit();
                        Logger.Info(output);
                    }

                    startInfo.Arguments = "checkout " + SHA1;

                    using (Process Proc = Process.Start(startInfo))
                    {
                        var output = Proc.StandardOutput.ReadToEnd();
                        Proc.WaitForExit();
                        Logger.Info(output);
                    }
                    startInfo.FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";
                    startInfo.Arguments = @"C:\inetpub\DOTNET\" + app.Name + @"\" + app.Name + @".sln" + @"/t:Build /p:Configuration=Release /p:TargetFramework=v4.0";

                    using (Process Proc = Process.Start(startInfo))
                    {
                        var output = Proc.StandardOutput.ReadToEnd();
                        Proc.WaitForExit();
                        Logger.Info(output);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Med("Pull and Build Failed:\n" + e.ToString());
                return Json(new {
                    msg = "failed",
                    error = e.ToString()
                });
            }
            Logger.Info("Pull and Build Success");
            return Json(new {
                msg = "success"
            });
            
        }

        // POST: Build/Remove
        [HttpPost]
        public ActionResult Remove(int id)
        {
            return View();
        }
    }
}