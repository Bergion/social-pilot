using Amazon;
using Amazon.SQS;
using InstagramApi.Api;
using InstagramApi.Config;
using InstagramApi.Controllers;
using InstagramApi.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDb"));
builder.Services.Configure<SqsConfig>(builder.Configuration.GetSection("Sqs"));
var awsSection = builder.Configuration.GetSection("AWS");
builder.Services.Configure<AwsConfig>(awsSection);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddTransient<IContainerApi, ContainerApi>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IAuthService, AuthService>();

var awsConfig = awsSection.Get<AwsConfig>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>(x => new AmazonSQSClient(awsConfig.AccessKey, awsConfig.SecretKey, RegionEndpoint.EUCentral1));

builder.Services.AddHostedService<PostQueueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
