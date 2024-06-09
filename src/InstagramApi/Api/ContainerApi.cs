using InstagramApi.Global.ApiRequests.ContainerRequests;
using InstagramApi.Global.ApiResponses;
using System.Net.Http.Json;

namespace InstagramApi.Api
{
    public interface IContainerApi
    {
        Task<string> CreateImageContainer(CreateImageContainerRequest request, string igUserId);
    }

    public class ContainerApi : IContainerApi
    {
        private readonly HttpClient _httpClient;

        public ContainerApi(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://graph.facebook.com/v20.0/");
        }

        public async Task<string> CreateImageContainer(CreateImageContainerRequest request, string igUserId)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"{igUserId}/media", request);

            responseMessage.EnsureSuccessStatusCode();

            var response = await responseMessage.Content.ReadFromJsonAsync<CreateContainerResponse>();

            return response?.Id;
        }
    }
}
