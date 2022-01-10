using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace todos.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Pass { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}