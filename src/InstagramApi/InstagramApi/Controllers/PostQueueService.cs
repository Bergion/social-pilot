using Amazon.SQS;
using Amazon.SQS.Model;
using InstagramApi.Config;
using InstagramApi.Global.Requests;
using InstagramApi.Service;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InstagramApi.Controllers
{
    public class PostQueueService : BackgroundService
    {
        private readonly IAmazonSQS _sqsService;
        private readonly SqsConfig _sqsConfig;
        private readonly IPostService _postService;
        private readonly IAuthService _authService;

        public PostQueueService(IAmazonSQS sqsService, IOptions<SqsConfig> sqsConfig, IPostService postService, IAuthService authService)
        {
            _sqsService = sqsService;
            _sqsConfig = sqsConfig.Value;
            _postService = postService;
            _authService = authService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ReceiveMessageRequest receiveMessageRequest = new(_sqsConfig.PostQueue)
                {
                    WaitTimeSeconds = 3
                };

                var messageResponse = await _sqsService.ReceiveMessageAsync(receiveMessageRequest);

                foreach (var message in messageResponse.Messages)
                {
                    var postRequest = JsonConvert.DeserializeObject<PostRequest>(message.Body);

                    ArgumentNullException.ThrowIfNull(postRequest);

                    var user = await _authService.GetAsync(postRequest.UserId);

                    ArgumentNullException.ThrowIfNull(user);

                    await _postService.CreatePostAsync(postRequest, user);

                    // Delete the message after processing
                    await _sqsService.DeleteMessageAsync(_sqsConfig.PostQueue, message.ReceiptHandle);
                }
            }
        }
    }
}
