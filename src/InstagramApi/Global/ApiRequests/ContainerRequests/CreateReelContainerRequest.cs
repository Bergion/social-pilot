namespace InstagramApi.Global.ApiRequests.ContainerRequests
{
    public class CreateReelContainerRequest : BaseApiRequest
    {
        public required string VideoUrl { get; set; }

        public string? Caption { get; set; }
    }
}
