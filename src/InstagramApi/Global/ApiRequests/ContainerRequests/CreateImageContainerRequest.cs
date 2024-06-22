using System.Text.Json.Serialization;

namespace InstagramApi.Global.ApiRequests.ContainerRequests
{
    public class CreateImageContainerRequest : BaseApiRequest
    {
        public required string ImageUrl { get; set; }

        public string? Caption { get; set; }
    }
}
