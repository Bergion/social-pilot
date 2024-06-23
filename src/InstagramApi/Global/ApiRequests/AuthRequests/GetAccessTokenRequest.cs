using Newtonsoft.Json;

namespace InstagramApi.Global.ApiRequests.AuthRequests
{
    public class GetAccessTokenRequest
    {
        [JsonProperty("client_id")]
        public required string AppId { get; set; }

        [JsonProperty("client_secret")]
        public required string AppSecret { get; set; }

        [JsonProperty("redirect_uri")]
        public required string RedirectUri { get; set; }

        [JsonProperty("code")]
        public required string Code { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType => "authorization_code";
    }
}
