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

            return platformData.PostType switch
            {
                Global.Enums.PostType.Post => await _containerApi.CreatePostContainerAsync(new CreatePostContainerRequest()
                {
                    AccessToken = user.Token,
                    Caption = request.Text,
                    IgUserId = user.IgUserId,
                    ImageUrl = request.Media[0].Url,
                }),
                Global.Enums.PostType.Story => await _containerApi.CreateStoryContainerAsync(new CreateStoryContainerRequest()
                {
                    AccessToken = user.Token,
                    IgUserId = user.IgUserId,
                    MediaUrl = request.Media[0].Url,
                }),
                Global.Enums.PostType.Reels => await _containerApi.CreateReelContainerAsync(new CreateReelContainerRequest()
                {
                    AccessToken = user.Token,
                    IgUserId = user.IgUserId,
                    Caption = request.Text,
                    VideoUrl = request.Media[0].Url,
                }),
                _ => throw new NotImplementedException($"Post type {platformData.PostType} is not supported"),
            };
        }

        private bool ContainsInstagram(PostRequest request)
        {
            return request.Platforms.Any(p => p.Name == Instagram);
        }
    }
}
