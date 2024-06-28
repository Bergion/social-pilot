namespace InstagramApi.Global.Requests
{
    public class CodeAuthenticationModel
    {
        public required string Code { get; set; }

        public required string UserId { get; set; }
    }
}
