using InstagramApi.Api;
using InstagramApi.Config;
using InstagramApi.Data.Models;
using InstagramApi.Global.ApiResponses;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Data;

namespace InstagramApi.Service
{
    public interface IAuthService
	{
        Task CreateUserOrUpdateTokenAsync(string userId, string igUserId, string token);
        Task<GetAccessTokenResponse> GetAccessTokenViaCodeAsync(string code);
        Task<User?> GetAsync(string userId);
    }

	public class AuthService : IAuthService
	{
		private readonly IMongoCollection<User> _users;
        private readonly IAuthApi _authApi;

        public AuthService(IOptions<MongoDbConfig> mongoConfig, IAuthApi authApi)
		{
			var mongoClient = new MongoClient(mongoConfig.Value.ConnectionString);

			var mongoDatabase = mongoClient.GetDatabase(mongoConfig.Value.DatabaseName);

			_users = mongoDatabase.GetCollection<User>(User.CollectionName);
            _authApi = authApi;
        }

		public async Task CreateUserOrUpdateTokenAsync(string userId, string igUserId, string token)
		{
            var user = await GetAsync(userId);

            if (user == null)
            {
                await CreateUserAsync(userId, igUserId, token);
            }
            else
            {
                await UpdateTokenAsync(userId, token);
            }
        }

		public async Task CreateUserAsync(string userId, string igUserId, string token)
		{
            var user = new User()
			{
				UserId = userId,
				IgUserId = igUserId,
				Token = token,
			};

			await _users.InsertOneAsync(user);
		}

		public async Task UpdateTokenAsync(string userId, string token)
		{
			var existingUser = await _users.Find(u => u.UserId == userId).FirstOrDefaultAsync();

            if (existingUser == null)
            {
				throw new DataException($"User with ID {userId} not found");
            }

            existingUser.Token = token;
			await _users.ReplaceOneAsync(u => u.UserId == userId, existingUser);
		}

		public async Task<User?> GetAsync(string userId) =>
			await _users.Find(u => u.UserId == userId).FirstOrDefaultAsync();

		public async Task<GetAccessTokenResponse> GetAccessTokenViaCodeAsync(string code)
		{
			return await _authApi.GetAccessTokenAsync(code);
		}
	}
}
