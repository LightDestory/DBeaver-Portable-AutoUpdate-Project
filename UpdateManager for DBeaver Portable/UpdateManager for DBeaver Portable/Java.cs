using System.Collections.Generic;

namespace UpdateManager_for_DBeaver_Portable
{
    class Java
    {
        public static readonly Dictionary<string, string> extractPath = new Dictionary<string, string>()
        {
            {"java32", @"App\java_32\"},
            {"java64", @"App\java_64\"}
        };
        public static readonly Dictionary<string, string> checkExistsFile = new Dictionary<string, string>()
        {
            {"java32", @"App\java_32\jre1.8.0_171\bin\javaw.exe"},
            {"java64", @"App\java_64\jre1.8.0_171\bin\javaw.exe"}
        };
        public static readonly Dictionary<string, string> downloadLinks = new Dictionary<string, string>()
        {
            {"java32", "http://download.oracle.com/otn-pub/java/jdk/8u171-b11/512cd62ec5174c3487ac17c61aaa89e8/jre-8u171-windows-i586.tar.gz"},
            {"java64", "http://download.oracle.com/otn-pub/java/jdk/8u171-b11/512cd62ec5174c3487ac17c61aaa89e8/jre-8u171-windows-x64.tar.gz"}
        };
        public static readonly string requiredCookie = "gpw_e24=http%3A%2F%2Fwww.oracle.com%2F;oraclelicense=accept-securebackup-cookie";
    }
}
