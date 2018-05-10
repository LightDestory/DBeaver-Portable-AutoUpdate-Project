using System.Collections.Generic;

namespace UpdateManager_for_DBeaver_Portable
{
    class DBeaver
    {
        public static readonly Dictionary<string, string> extractPath = new Dictionary<string, string>()
        {
            {"dbeaver32", @"App\dbeaver32\"},
            {"dbeaver64", @"App\dbeaver64\"}

        };
        public static readonly Dictionary<string, string> checkExistsFile = new Dictionary<string, string>()
        {
            {"dbeaver32", @"App\dbeaver32\dbeaver\dbeaver.exe"},
            {"dbeaver64", @"App\dbeaver64\dbeaver\dbeaver.exe"}
        };
        public static readonly Dictionary<string, string> launcherFile = new Dictionary<string, string>()
        {
            {"32", @"DBeaver32Portable.exe"},
            {"64", @"DBeaver64Portable.exe"}

        };
        public static readonly Dictionary<string, string> downloadLinks = new Dictionary<string, string>()
        {
            {"dbeaver32", "https://dbeaver.io/files/dbeaver-ce-latest-win32.win32.x86.zip"},
            {"dbeaver64", "https://dbeaver.io/files/dbeaver-ce-latest-win32.win32.x86_64.zip"}

        };
        public static readonly Dictionary<string, string> configFiles = new Dictionary<string, string>()
        {
            {"LauncherINI", @"App\AppInfo\Launcher\DBeaverPortable.ini"},
            {"AppInfoINI", @"App\AppInfo\AppInfo.ini"}
        };
        public static readonly Dictionary<string, string> htmlScraping = new Dictionary<string, string>()
        {
            {"pattern", "<h2>Community Edition .{5}</h2>"},
            {"pre", "<h2>Community Edition "},
            {"suff", "</h2>"}
        };
        public static readonly string versionLink = "https://dbeaver.io/download/";

    }
}
