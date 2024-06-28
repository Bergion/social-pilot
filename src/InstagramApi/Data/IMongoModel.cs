namespace InstagramApi.Data
{
	internal interface IMongoModel
	{
		static string? CollectionName { get; }
	}
}
