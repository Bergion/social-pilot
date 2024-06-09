using InstagramApi.Api;
using InstagramApi.Config;
using InstagramApi.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDb"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IContainerApi, ContainerApi>();

builder.Services.AddSingleton<IAuthService, AuthService>();

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
