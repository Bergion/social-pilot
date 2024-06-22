namespace InstagramApi.Global.ApiRequests
{
    public abstract class BaseApiRequest
    {
        public required string AccessToken { get; set; }

        public required string IgUserId { get; set; }
    }
}
