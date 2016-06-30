using LSAppManagementWebservice.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LSAppManagementWebservice.Helpers
{
    public static class AppBuildHelper
    {
        public static void PullApp(ApplicationModel app, string SHA1)
        {
            string AppPath = @"C:/inetpub\DOTNET\" + app.Name;

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
            startInfo.Arguments = "init --shared=all \"" + AppPath + "\"";

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
            startInfo.Arguments = "clone " + app.GithubURL.ToString() + " \"" + AppPath + "\"";

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
        }

        public static void BuildApp(ApplicationModel app, string SHA1)
        {
            string AppPath = @"C:/inetpub\DOTNET\" + app.Name;

            if (Directory.Exists(AppPath + @"\packages\"))
            {
                Delete(new DirectoryInfo(AppPath + @"\packages\"));
            }

            if (System.IO.File.Exists(AppPath + @"\.nuget\Nuget.exe"))
            {
                System.IO.File.Delete(AppPath + @"\.nuget\Nuget.exe");
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = "msbuild";
            startInfo.Arguments = "\"" + AppPath + @"\" + app.Name + ".sln\"" + @" /p:DeployOnBuild=true;PublishProfile=C:\inetpub\DOTNET\filesystem_publish.pubxml;Configuration=Release;VisualStudioVersion=12.0";

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
        }

        public static void Delete(DirectoryInfo Dir)
        {
            if (!Dir.Exists)
                return;

            foreach (var SubDir in Dir.EnumerateDirectories())
            {
                Delete(SubDir);
            }

            try
            {
                Dir.Delete(true);
            }
            catch (IOException e)
            {
                string fileName = Dir.FullName;

                Process tool = new Process();
                tool.StartInfo.FileName = @"C:\handle.exe";
                tool.StartInfo.Arguments = fileName + " /accepteula";
                tool.StartInfo.UseShellExecute = false;
                tool.StartInfo.RedirectStandardOutput = true;
                tool.Start();
                tool.WaitForExit();
                string outputTool = tool.StandardOutput.ReadToEnd();

                string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
                foreach (Match match in Regex.Matches(outputTool, matchPattern))
                {
                    Process.GetProcessById(int.Parse(match.Value)).Kill();
                }

                Delete(Dir);
            }
        }
    }
}