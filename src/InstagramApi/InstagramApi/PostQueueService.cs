using Amazon.SQS;
using Amazon.SQS.Model;
using InstagramApi.Global.Requests;
using Newtonsoft.Json;

namespace InstagramApi
{
    public class PostQueueService : BackgroundService
    {
        private readonly IAmazonSQS _sqsService;
        private readonly string _queueUrl = "";

        public PostQueueService(IAmazonSQS sqsService)
        {
            _sqsService = sqsService;
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

                    // Delete the message after processing
                    await _sqsService.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                }
            }
        }
    }
}
