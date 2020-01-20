
using MTGBot.Models;
using Newtonsoft.Json;
using VTFileSystemManagement;

namespace MTGBot.Data.ReadWriteJSON
{
    public static class BotToken
    {
        public static string GetTokenString()
        {
            FileSystemManager fileSystem = new FileSystemManager();
            var token = fileSystem.ReadJsonFile("APITokens.json");
            var deserialized = JsonConvert.DeserializeObject<TokenModel>(token);

            return deserialized.MTGBotToken;
        }
    }
}
