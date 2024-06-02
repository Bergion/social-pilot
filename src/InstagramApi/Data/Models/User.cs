using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InstagramApi.Data.Models
{
	internal class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? UserId { get; set; }

        public required string Token { get; set; }
    }
}
