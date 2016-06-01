using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSAppManagementWebservice.Helpers
{
    public static class IISHelper
    {
        public static bool AppExists(string SiteName, string AppName)
        {
            using (var SvrMgr = new ServerManager())
            {
                var Sites = SvrMgr.Sites;

                if (Sites[SiteName] == null)
                {
                    return false;
                }

                var Apps = Sites[SiteName].Applications;

                if (Apps["/" + AppName] == null)
                {
                    return false;
                }

                return true;
            }
        }

        public static bool AddApp(string SiteName, string AppName, string AppWebPath, string AppPhysicalPath)
        {
            using (var SvrMgr = new ServerManager())
            {
                try
                {
                    var Sites = SvrMgr.Sites;
                    if(Sites[SiteName] == null){
                        return false;
                    }
                    var WebSite = Sites[SiteName];
                    var Apps = WebSite.Applications;

                    if (Apps["/" + AppName] == null)
                    {
                        Apps.Add("/" + AppWebPath, AppPhysicalPath);
                        SvrMgr.CommitChanges();
                        return true;
                    }
                    else
                    {
                        Logger.Med("Error adding app: App " + AppName + " already exists");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Logger.Med("Error adding app-" + AppName + ":\n" + e.ToString() + "\n" + e.StackTrace);
                    return false;
                }
            }
        }

        public static bool RemoveApp(string SiteName, string AppName)
        {
            using (var SvrMgr = new ServerManager())
            {
                try
                {
                    var WebSite = SvrMgr.Sites.SingleOrDefault(s => s.Name == SiteName);
                    var Apps = WebSite.Applications;
                    if (Apps["/" + AppName] != null)
                    {
                        Apps.Remove(Apps["/" + AppName]);
                        SvrMgr.CommitChanges();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Med("Error deleting app-" + AppName + ":\n" + e.ToString() + "\n" + e.StackTrace);
                    return false;
                }
            }
        }
    }
}