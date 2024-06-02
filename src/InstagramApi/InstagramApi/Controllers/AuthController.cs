using InstagramApi.ViewModels.Requests;
using Microsoft.AspNetCore.Mvc;

namespace InstagramApi.Controllers
{
	public class AuthController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> SetToken(SetTokenModel model)
		{

		}
	}
}
