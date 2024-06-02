using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InstagramApi.Data.Models
{
	public class User : IMongoModel
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? UserId { get; set; }

        public required string Token { get; set; }

		public static string CollectionName => "users";
	}
}
