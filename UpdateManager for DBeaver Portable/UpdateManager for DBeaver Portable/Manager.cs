using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;

namespace UpdateManager_for_DBeaver_Portable
{
    class Manager
    {
        private string LocalVersion = null, OnlineVersion = null, current_dir, PortableType;
        private Downloader Network;
        private int i;

        public Manager()
        {
            current_dir = AppDomain.CurrentDomain.BaseDirectory;
            Network = new Downloader();
            RetrievePortableType();
        }

        public bool isUpdateAvailable()
        {
            RetrieveLocalVersion();
            RetrieveOnlineVersion();
            return !LocalVersion.Equals(OnlineVersion);
        }

        private void Download(string name, string link)
        {
            try
            {
                ConsoleUtils.WriteLineColored("Downaloding " + name, "warn");
                if(name.Contains("java"))
                {
                    Network.SetCookie(Java.requiredCookie);
                }
               Network.DownloadFile(link, Path.GetTempPath() + name);
                while (!Network.DownloadCompleted)
                    Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                ConsoleUtils.ErrorCall("An Error occured while downloading " + name + " !\nPlease Contact the developer and sent this a screenshot of me!\n\n " + ex.ToString());
            }
        }

        private void UnPackDBeaver(string name, string path)
        {
            try
            {
                Console.WriteLine("");
                FastZip c = new FastZip();
                c.ExtractZip(Path.GetTempPath() + name, path, null);
                ConsoleUtils.WriteLineColored("Extraction completed!\nDeleting downloaded zip...", "info");
                File.Delete(Path.GetTempPath() + name);
            }
            catch (Exception ex)
            {
                ConsoleUtils.ErrorCall("An Error occured while applying Update!\nPlease Contact the developer and sent this a screenshot of me!\n\n " + ex.ToString());
            }
        }

        private void UnPackJRE(string name, string path)
        {
            Stream inStream = File.OpenRead(Path.GetTempPath() + name);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(path);
            tarArchive.Close();
            gzipStream.Close();
            inStream.Close();
        }

        public void StartProgram()
        {
            try
            {
                if (IntPtr.Size==8)
                {
                    switch (PortableType)
                    {
                        case "64":
                        case "both":
                            ConsoleUtils.WriteLineColored("Running DBeaver 64bit...", "info");
                            Process.Start(current_dir + DBeaver.launcherFile["64"]);
                            break;
                        default: //32
                            ConsoleUtils.WriteLineColored("No DBeaver 64bit binaries available...\nRunning DBeaver 32bit over a 64bit System...", "warn");
                            Process.Start(current_dir + DBeaver.launcherFile["32"]);
                            break;
                    }
                }
                else
                {
                    switch (PortableType)
                    {
                        case "32":
                        case "both":
                            ConsoleUtils.WriteLineColored("Running DBeaver 32bit...", "info");
                            Process.Start(current_dir + DBeaver.launcherFile["32"]);
                            break;
                        default: //64
                            ConsoleUtils.WriteLineColored("No DBeaver 32bit binaries available...", "error");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleUtils.ErrorCall("An Error occured while starting DBeaver!\nPlease Contact the developer and sent a screenshot of me!\n\n " + ex.ToString());
            }
        }

        public void CheckJavaStatus()
        {
            switch (PortableType)
            {
                case "32":
                    if(File.Exists(current_dir+Java.checkExistsFile["java32"]))
                    {
                        ConsoleUtils.WriteLineColored("Detected Java JRE 32bit!", "info");
                    }
                    else
                    {
                        ConsoleUtils.WriteLineColored("No Java JRE 32bit detected!\n It is required to run DBeaver!\n Starting downloading...", "warn");
                        Download("java-jre-32.tar.gz", Java.downloadLinks["java32"]);
                        UnPackJRE("java-jre-32.tar.gz", current_dir+Java.extractPath["java32"]);
                    }
                    break;
                case "64":
                    if (File.Exists(current_dir + Java.checkExistsFile["java32"]))
                    {
                        ConsoleUtils.WriteLineColored("Detected Java JRE 64bit!", "info");
                    }
                    else
                    {
                        ConsoleUtils.WriteLineColored("No Java JRE 64bit detected!\n It is required to run DBeaver!\n Starting downloading...", "warn");
                        Download("java-jre-64.tar.gz", Java.downloadLinks["java64"]);
                        UnPackJRE("java-jre-64.tar.gz", current_dir + Java.extractPath["java64"]);
                    }
                    break;
                default: // both
                    if (!File.Exists(current_dir + Java.checkExistsFile["java32"]))
                    {
                        ConsoleUtils.WriteLineColored("No Java JRE 32bit detected!\n It is required to run DBeaver!\n Starting downloading...", "warn");
                        Download("java-jre-32.tar.gz", Java.downloadLinks["java32"]);
                        UnPackJRE("java-jre-32.tar.gz", current_dir + Java.extractPath["java32"]);
                    }
                    if (!File.Exists(current_dir + Java.checkExistsFile["java64"]))
                    {
                        ConsoleUtils.WriteLineColored("No Java JRE 64bit detected!\n It is required to run DBeaver!\n Starting downloading...", "warn");
                        Download("java-jre-64.tar.gz", Java.downloadLinks["java64"]);
                        UnPackJRE("java-jre-64.tar.gz", current_dir + Java.extractPath["java64"]);
                    }
                    break;
            }
            ConsoleUtils.WriteLineColored("JAVA OK!", "info");
        }

        public void CheckDBeaverStatus()
        {
            switch (PortableType)
            {
                case "32":
                    ConsoleUtils.WriteLineColored("Detected DBeaver 32bit Portable!", "info");
                    break;
                case "64":
                    ConsoleUtils.WriteLineColored("Detected DBeaver 64bit Portable!", "info");
                    break;
                case "both":
                    ConsoleUtils.WriteLineColored("Detected DBeaver 32bit-64bit Portable!", "info");
                    break;
                default: // no
                    ConsoleUtils.WriteLineColored("Can't detect any DBeaver Portable binaries!\n Please re-download the package!\n Press any key to exit...", "error");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
            }
        }

        private void RefreshLocalVersion()
        {
            try
            {
                string[] temp = File.ReadAllLines(current_dir + DBeaver.configFiles["AppInfoINI"]);
                for (i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Contains("PackageVersion"))
                    {
                        temp[i] = "PackageVersion=" + OnlineVersion + ".0";
                    }
                    else if (temp[i].Contains("DisplayVersion"))
                    {
                        temp[i] = "DisplayVersion=" + OnlineVersion;
                    }
                }
                File.WriteAllLines(current_dir + DBeaver.configFiles["AppInfoINI"], temp);
            }
            catch (Exception ex)
            {
                ConsoleUtils.ErrorCall("An Error occured while updating AppInfo file!\nPlease Contact the developer and sent a screenshot of me!\n\n " + ex.ToString());
            }
        }

        private void RetrieveLocalVersion()
        {
            try
            {
                string[] temp = File.ReadAllLines(current_dir + DBeaver.configFiles["AppInfoINI"]);
                for (i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Contains("DisplayVersion"))
                    {
                        LocalVersion = temp[i].Replace("DisplayVersion=", "");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleUtils.ErrorCall("An Error occured while retriving DBeaver's local version number!\nPlease Contact the developer and sent a screenshot of me!\n\n " + ex.ToString());
            }
        }

        private void RetrieveOnlineVersion()
        {
            try
            {
                string temp = Network.client.DownloadString(DBeaver.versionLink);
                OnlineVersion = Regex.Match(temp, DBeaver.htmlScraping["pattern"]).Value.Replace(DBeaver.htmlScraping["pre"], "").Replace(DBeaver.htmlScraping["suff"], "");
            }
            catch (Exception ex)
            {
                ConsoleUtils.ErrorCall("An Error occured while retriving DBeaver's latest version number from website!\nPlease Contact the developer and sent a screenshot of me!\n\n " + ex.ToString());
            }
        }

        private void RetrievePortableType()
        {
            if (File.Exists(current_dir + DBeaver.checkExistsFile["dbeaver32"]) && !File.Exists(current_dir + DBeaver.checkExistsFile["dbeaver64"]))
            {
                PortableType = "32";
            }
            else if (!File.Exists(current_dir + DBeaver.checkExistsFile["dbeaver32"]) && File.Exists(current_dir + DBeaver.checkExistsFile["dbeaver64"]))
            {
                PortableType = "64";
            }
            else if (File.Exists(current_dir + DBeaver.checkExistsFile["dbeaver32"]) && File.Exists(current_dir + DBeaver.checkExistsFile["dbeaver64"]))
            {
                PortableType = "both";
            }
            else
            {
                PortableType = "no";
            }
        }

        public bool CheckConnection()
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send("104.236.93.175", 10000, new byte[32], new PingOptions());
                // 104.236.93.175 = www.dbeaver.io
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
