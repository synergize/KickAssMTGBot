using System;

namespace MTGBot.Helpers
{
    public static class ConsoleWriteOverride
    {
        public static string AddTimeStamp(string consoleMessage)
        {
            return $"[[{DateTime.Now.ToString("HH:mm:ss")}]]: {consoleMessage}";
        }
    }
}
