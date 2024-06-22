using Amazon.SQS;
using Amazon.SQS.Model;
using InstagramApi.Global.Requests;
using InstagramApi.Service;
using Newtonsoft.Json;

namespace InstagramApi.Controllers
{
    public class PostQueueService : BackgroundService
    {
        private readonly IAmazonSQS _sqsService;
        private readonly IPostService _postService;
        private readonly string _queueUrl = "";

        public PostQueueService(IAmazonSQS sqsService, IPostService postService)
        {
            _sqsService = sqsService;
            _postService = postService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ReceiveMessageRequest receiveMessageRequest = new(_queueUrl)
                {
                    WaitTimeSeconds = 3
                };

                var messageResponse = await _sqsService.ReceiveMessageAsync(receiveMessageRequest);

                foreach (var message in messageResponse.Messages)
                {
                    var postRequest = JsonConvert.DeserializeObject<PostRequest>(message.Body);

                    ArgumentNullException.ThrowIfNull(postRequest);

                    await _postService.CreatePostAsync(postRequest);

                    // Delete the message after processing
                    await _sqsService.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                }
            }
        }
    }
}
