namespace InstagramApi.Global.ApiRequests.ContainerRequests
{
    public class CreatePostContainerRequest : BaseApiRequest
    {
        public required string ImageUrl { get; set; }

        public string? Caption { get; set; }
    }
}
