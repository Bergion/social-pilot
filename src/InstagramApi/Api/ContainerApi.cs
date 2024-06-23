using InstagramApi.Global.ApiRequests.ContainerRequests;
using InstagramApi.Global.ApiResponses;
using System.Net.Http.Json;

namespace InstagramApi.Api
{
    public interface IContainerApi
    {
        Task<string> CreateImageContainerAsync(CreatePostContainerRequest request);
        Task PublishContainerAsync(PublishContainerRequest request);
    }

    public class ContainerApi : IContainerApi
    {
        private readonly HttpClient _httpClient;

        public ContainerApi(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://graph.facebook.com/v20.0/");
        }

        public async Task<string> CreateImageContainerAsync(CreatePostContainerRequest request)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, $"{request.IgUserId}/media");

            req.Content = new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                { "access_token", request.AccessToken },
                { "image_url", request.ImageUrl },
                { "caption", request.Caption },
            });

            var responseMessage = await _httpClient.SendAsync(req);

            responseMessage.EnsureSuccessStatusCode();

            var response = await responseMessage.Content.ReadFromJsonAsync<CreateContainerResponse>();

            return response?.Id;
        }

        public async Task PublishContainerAsync(PublishContainerRequest request)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, $"{request.IgUserId}/media");

            req.Content = new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                { "access_token", request.AccessToken },
                { "creation_id", request.CreationId },
            });

            var responseMessage = await _httpClient.SendAsync(req);

            responseMessage.EnsureSuccessStatusCode();
        }
    }
}
