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
		public async Task<IActionResult> AuthenticateViaCode(CodeAuthenticationModel model)
		{
            var response = await _authService.GetAccessTokenViaCodeAsync(model.Code);

            await _authService.CreateUserOrUpdateTokenAsync(model.UserId, response.IgUserId, response.AccessToken);

            return Ok();
		}
	}
}
