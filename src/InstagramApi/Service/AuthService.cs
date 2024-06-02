using InstagramApi.Config;
using InstagramApi.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InstagramApi.Service
{
	public interface IAuthService
	{
		Task CreateOrUpdateAsync(User user);
		Task<User?> GetAsync(string userId);
	}

	public class AuthService : IAuthService
	{
		private readonly IMongoCollection<User> _users;

		public AuthService(IOptions<MongoDbConfig> mongoConfig)
		{
			var mongoClient = new MongoClient(mongoConfig.Value.ConnectionString);

			var mongoDatabase = mongoClient.GetDatabase(mongoConfig.Value.DatabaseName);

			_users = mongoDatabase.GetCollection<User>(User.CollectionName);
		}

		public async Task CreateOrUpdateAsync(User user)
		{
			var existingUser = await _users.FindAsync(u => u.UserId == user.UserId);

			if (existingUser == null)
			{
				await _users.InsertOneAsync(user);
			}
			else
			{
				await _users.ReplaceOneAsync(u => u.UserId == user.UserId, user);
			}
		}

		public async Task<User?> GetAsync(string userId) =>
			await _users.Find(u => u.UserId == userId).FirstOrDefaultAsync();
	}
}
