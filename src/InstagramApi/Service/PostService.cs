using InstagramApi.Global.Requests;

namespace InstagramApi.Service
{
    public class PostService
    {
        private const string Instagram = "Instagram";

        public async Task CreatePostAsync(PostRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            
        }

        private bool ContainsInstagram(PostRequest request)
        {
            return request.Platforms.Any(p => p.Name == Instagram);
        }
    }
}
