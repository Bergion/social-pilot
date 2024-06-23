using Newtonsoft.Json;

namespace InstagramApi.Global.ApiResponses
{
    public class GetAccessTokenResponse
    {
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }

        [JsonProperty("user_id")]
        public required string IgUserId { get; set; }
    }
}
