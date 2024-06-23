using InstagramApi.Config;
using InstagramApi.Global.ApiRequests.AuthRequests;
using InstagramApi.Global.ApiResponses;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace InstagramApi.Api
{
    public interface IAuthApi
    {
        Task<GetAccessTokenResponse> GetAccessTokenAsync(string code);
    }

    public class AuthApi : IAuthApi
    {
        private readonly HttpClient _httpClient;
        private readonly InstagramAuthConfig _authConfig;

        public AuthApi(IHttpClientFactory httpClientFactory, IOptions<InstagramAuthConfig> instagramAuthConfig)
        {
            _httpClient = httpClientFactory.CreateClient();
            _authConfig = instagramAuthConfig.Value;
        }

        public async Task<GetAccessTokenResponse> GetAccessTokenAsync(string code)
        {
            var request = new GetAccessTokenRequest()
            {
                Code = code,
                AppId = _authConfig.AppId,
                AppSecret = _authConfig.AppSecret,
                RedirectUri = _authConfig.RedirectUrl,
            };

            var responseMessage = await _httpClient.PostAsJsonAsync("https://api.instagram.com/oauth/access_token", request);

            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<GetAccessTokenResponse>();
        }
    }
}
