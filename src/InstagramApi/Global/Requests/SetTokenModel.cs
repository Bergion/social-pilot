namespace InstagramApi.Global.Requests
{
    public class SetTokenModel
    {
        public required string UserId { get; set; }

        public required string IgUserId { get; set; }

        public required string Token { get; set; }
    }
}
