using System;
using System.Collections.Generic;
using System.Text;

namespace MTGBot.Helpers
{
    public static class FormatUserInput
    {
        public static string FormatEntry(string entry)
        {
            if (entry.Contains('[') || entry.Contains(']'))
            {
                int count = 0;
                entry = entry.TrimStart().TrimEnd().ToUpper();
                if (entry.Contains('?'))
                {
                    entry = entry.Remove(0, 3);
                }
                else
                {
                    entry = entry.Remove(0, 2);
                }
                count = entry.Length - 2;
                entry = entry.Remove(count, 2);
                if (entry.Contains(" "))
                {
                    entry = entry.Replace(" ", "+");
                }
            }
            return entry;
        }
    }
}
