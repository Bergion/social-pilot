namespace InstagramApi.Api
{
	public class PostApi
	{
		HttpClient _httpClient;

		public PostApi(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
			_httpClient.BaseAddress = new Uri("https://graph.facebook.com/v20.0/");
		}


	}
}
