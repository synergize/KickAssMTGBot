namespace MTGBot.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }
    }
}
