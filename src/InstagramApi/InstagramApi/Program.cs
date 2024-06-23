using Amazon.SQS;
using InstagramApi.Api;
using InstagramApi.Config;
using InstagramApi.Controllers;
using InstagramApi.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDb"));
builder.Services.Configure<SqsConfig>(builder.Configuration.GetSection("Sqs"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddTransient<IContainerApi, ContainerApi>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IAuthService, AuthService>();

var options = builder.Configuration.GetAWSOptions();
var client = options.CreateServiceClient<IAmazonSQS>();

builder.Services.AddSingleton(client);

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
