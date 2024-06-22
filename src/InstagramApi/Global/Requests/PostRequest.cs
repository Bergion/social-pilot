using InstagramApi.Global.Enums;

namespace InstagramApi.Global.Requests
{
    public class PostRequest
    {
        public required string UserId { get; set; }

        public string? Text { get; set; }

        public IReadOnlyList<MediaData> Media { get; set; } = [];

        public IReadOnlyList<PlatformData> Platforms { get; set; } = [];

        public record MediaData
        {
            public required string Url { get; set; }
        }

        public record PlatformData
        {
            public int Id { get; set; }

            public required string Name { get; set; }

            public PostType PostType { get; set; }
        }
    }
}
