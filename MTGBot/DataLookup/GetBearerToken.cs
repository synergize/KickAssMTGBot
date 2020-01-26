using MTGBot.APICredentials;
namespace MTGBot.DataLookup
{
    public static class GetBearerToken
    {
        private static string PublicKey = APIObject.PublicKey;
        private static string PrivateKey = APIObject.PrivateKey;
        private static string Version = APIObject.Version;
        //public static string GetBearer()
        //{
        //    //var client = new RestClient();
        //    //client.BaseUrl = new Uri("https://api.tcgplayer.com/");

        //    //var request = new RestRequest(Method.POST);
        //    //request.Resource = "/token";
        //    //request.RequestFormat = DataFormat.Json;
        //    //request.AddHeader("Content-Type", "application/json");

        //    //request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=" + PublicKey + "&client_secret=" + PrivateKey, ParameterType.RequestBody);

        //    //var tcgResponse = client.Execute(request);
        //    //var r = JsonConvert.DeserializeObject<dynamic>(tcgResponse.Content);

        //    //return r.access_token;
        //}
    }
}
