using InstagramApi.Api;
using InstagramApi.Data.Models;
using InstagramApi.Global.Requests;

namespace InstagramApi.Service
{
    public interface IPostService
    {
        Task CreatePostAsync(PostRequest request, User user);
    }

    public class PostService : IPostService
    {
        private const string Instagram = "Instagram";

        private readonly IContainerApi _containerApi;

        public PostService(IContainerApi containerApi)
        {
            _containerApi = containerApi;
        }

        public async Task CreatePostAsync(PostRequest request, User user)
        {
            ArgumentNullException.ThrowIfNull(request);

            var containerId = await _containerApi.CreateImageContainerAsync();
        }

        private bool ContainsInstagram(PostRequest request)
        {
            return request.Platforms.Any(p => p.Name == Instagram);
        }
    }
}
