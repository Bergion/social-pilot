using InstagramApi.Global.ApiRequests.ContainerRequests;
using InstagramApi.Global.ApiResponses;
using InstagramApi.Global.Helpers;
using System.Net.Http.Json;

namespace InstagramApi.Api
{
    public interface IContainerApi
    {
        Task<string> CreatePostContainerAsync(CreatePostContainerRequest request);
        Task<string> CreateReelContainerAsync(CreateReelContainerRequest request);
        Task<string> CreateStoryContainerAsync(CreateStoryContainerRequest request);
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

        public async Task<string> CreatePostContainerAsync(CreatePostContainerRequest request)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, $"{request.IgUserId}/media");

            req.Content = new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                { "access_token", request.AccessToken },
                { "image_url", request.ImageUrl },
                { "caption", request.Caption },
            });

            return await CreateContainerAsync(req);
        }

        public async Task<string> CreateStoryContainerAsync(CreateStoryContainerRequest request)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, $"{request.IgUserId}/media");

            var mediaType = MediaTypeHelper.GetMediaType(request.MediaUrl);

            if (mediaType == Global.Enums.MediaType.Other)
            {
                throw new ArgumentException("Unsupported media type", nameof(request.MediaUrl));
            }

            var mediaParamName = mediaType == Global.Enums.MediaType.Video ? "video_url" : "image_url";

            req.Content = new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                { "access_token", request.AccessToken },
                { mediaParamName, request.MediaUrl },
                { "media_type", "STORIES" },
            });

            return await CreateContainerAsync(req);
        }

        public async Task<string> CreateReelContainerAsync(CreateReelContainerRequest request)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, $"{request.IgUserId}/media");

            req.Content = new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                { "access_token", request.AccessToken },
                { "video_url", request.VideoUrl },
                { "caption", request.Caption },
                { "media_type", "REELS" },
            });

            return await CreateContainerAsync(req);
        }

        private async Task<string> CreateContainerAsync(HttpRequestMessage request)
        {
            var responseMessage = await _httpClient.SendAsync(request);

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
