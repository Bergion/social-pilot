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

            var containerId = await CreateContainerAsync(request, user);

            var publishContainerRequest = new PublishContainerRequest()
            {
                IgUserId = user.IgUserId,
                AccessToken = user.Token,
                CreationId = containerId,
            };

            await _containerApi.PublishContainerAsync(publishContainerRequest);
        }

        private async Task<string> CreateContainerAsync(PostRequest request, User user)
        {
            var platformData = request.Platforms.First(p => p.Name == Instagram);

            switch (platformData.PostType)
            {
                case Global.Enums.PostType.Post:
                    var containerRequest = new CreatePostContainerRequest()
                    {
                        AccessToken = user.Token,
                        Caption = request.Text,
                        IgUserId = user.IgUserId,
                        ImageUrl = request.Media[0].Url,
                    };

                    return await _containerApi.CreateImageContainerAsync(containerRequest);

                case Global.Enums.PostType.Story:
                    break;
                case Global.Enums.PostType.Reels:
                    break;
                default:
                    throw new NotImplementedException($"Post type {platformData.PostType} is not supported");
            }
        }

        private bool ContainsInstagram(PostRequest request)
        {
            return request.Platforms.Any(p => p.Name == Instagram);
        }
    }
}
