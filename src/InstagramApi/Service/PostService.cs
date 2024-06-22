using InstagramApi.Api;
using InstagramApi.Data.Models;
using InstagramApi.Global.ApiRequests.ContainerRequests;
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

            if (!ContainsInstagram(request))
            {
                return;
            }

            var containerRequest = new CreateImageContainerRequest()
            {
                AccessToken = user.Token,
                Caption = request.Text,
                IgUserId = user.IgUserId,
                ImageUrl = request.Media[0].Url
            };

            var containerId = await _containerApi.CreateImageContainerAsync(containerRequest);

            var publishContainerRequest = new PublishContainerRequest()
            {
                IgUserId = user.IgUserId,
                AccessToken = user.Token,
                CreationId = containerId,
            };

            await _containerApi.PublishContainerAsync(publishContainerRequest);
        }

        private bool ContainsInstagram(PostRequest request)
        {
            return request.Platforms.Any(p => p.Name == Instagram);
        }
    }
}
