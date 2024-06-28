using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InstagramApi.Data.Models
{
	public class User : IMongoModel
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public required string UserId { get; set; }

        public required string Token { get; set; }

		public required string IgUserId { get; set; }

		public static string CollectionName => "users";
	}
}
