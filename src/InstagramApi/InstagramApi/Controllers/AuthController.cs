using InstagramApi.Global.Requests;
using InstagramApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace InstagramApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
	{
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
		public async Task<IActionResult> SetToken(SetTokenModel model)
		{
            await _authService.CreateUserOrUpdateTokenAsync(model.UserId, model.IgUserId, model.Token);

            return Ok();
		}
	}
}
