using System;

namespace UpdateManager_for_DBeaver_Portable
{
    class ConsoleUtils
    {
        public static void WriteLineColored(String text, String colorset)
        {
            ConsoleColor font, back;
            switch (colorset)
            {
                case "warn":
                    back = ConsoleColor.Yellow;
                    font = ConsoleColor.DarkBlue;
                    break;
                case "error":
                    back = ConsoleColor.DarkRed;
                    font = ConsoleColor.Black;
                    break;
                default: // info
                    back = ConsoleColor.DarkGray;
                    font = ConsoleColor.White;
                    break;
            }
            Console.BackgroundColor = back;
            Console.ForegroundColor = font;
            Console.Out.WriteLine(" " + text + " ");
            Console.ResetColor();
        }

        public static void ErrorCall(String text)
        {
            WriteLineColored(text, "error");
            Console.Out.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
