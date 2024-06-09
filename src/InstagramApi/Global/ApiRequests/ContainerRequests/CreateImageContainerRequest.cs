using System.Text.Json.Serialization;

namespace InstagramApi.Global.ApiRequests.ContainerRequests
{
    public class CreateImageContainerRequest : BaseApiRequest
    {
        [JsonPropertyName("image_url")]
        public required string ImageUrl { get; set; }

        [JsonPropertyName("caption")]
        public string? Caption { get; set; }
    }
}
