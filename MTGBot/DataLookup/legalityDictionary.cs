using System.Collections.Generic;

namespace MTGBot.DataLookup
{
    static class LegalityDictionary
    {
        public static Dictionary<string, string> Legality = new Dictionary<string, string>();
        public static void LoadLegalityDict()
        {
            if (Legality.Count == 0)
            {
                Legality.Add("not_legal", "Not Legal");
                Legality.Add("legal", "Legal");
                Legality.Add("banned", "Banned");
                Legality.Add("restricted", "Restricted");
            }
        }
    }
}
