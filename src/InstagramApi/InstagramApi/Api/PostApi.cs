namespace InstagramApi.Api
{
	public class PostApi
	{
		HttpClient _httpClient;

        public PostApi(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }


    }
}
