using Microsoft.AspNetCore.Mvc;

namespace InstagramApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PostApi : ControllerBase
	{
		private readonly ILogger<PostApi> _logger;

		public PostApi(ILogger<PostApi> logger)
		{
			_logger = logger;
		}


	}
}