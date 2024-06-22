using Amazon.SQS;
using InstagramApi.Config;
using InstagramApi.Global.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InstagramApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IAmazonSQS _sqsService;
        private readonly SqsConfig _sqsConfig;

        public TestController(IAmazonSQS sqsService, IOptions<SqsConfig> sqsConfig)
        {
            _sqsService = sqsService;
            _sqsConfig = sqsConfig.Value;
        }

        [HttpPost]
        public async Task<IActionResult> PushToQueue(PostRequest postRequest)
        {
            await _sqsService.SendMessageAsync(_sqsConfig.PostQueue, JsonConvert.SerializeObject(postRequest));

            return Ok();
        }
    }
}
