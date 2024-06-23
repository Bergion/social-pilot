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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly SqsConfig _sqsConfig;

        public PostQueueService(IAmazonSQS sqsService, IOptions<SqsConfig> sqsConfig, IServiceScopeFactory serviceScopeFactory)
        {
            _sqsService = sqsService;
            _serviceScopeFactory = serviceScopeFactory;
            _sqsConfig = sqsConfig.Value;
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

                if (messageResponse.Messages.Count == 0)
                {
                    continue;
                }

                using var scope = _serviceScopeFactory.CreateScope();

                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var postService = scope.ServiceProvider.GetRequiredService<IPostService>();

                foreach (var message in messageResponse.Messages)
                {
                    var postRequest = JsonConvert.DeserializeObject<PostRequest>(message.Body);

                    ArgumentNullException.ThrowIfNull(postRequest);

                    var user = await authService.GetAsync(postRequest.UserId);

                    ArgumentNullException.ThrowIfNull(user);

                    await postService.CreatePostAsync(postRequest, user);

                    // Delete the message after processing
                    await _sqsService.DeleteMessageAsync(_sqsConfig.PostQueue, message.ReceiptHandle);
                }
            }
        }
    }
}
