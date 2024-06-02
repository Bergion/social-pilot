using Microsoft.AspNetCore.Mvc;

namespace InstagramApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PostController : ControllerBase
	{
		private readonly ILogger<PostController> _logger;

		public PostController(ILogger<PostController> logger)
		{
			_logger = logger;
		}


	}
}