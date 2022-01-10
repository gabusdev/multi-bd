using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace todos.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = null!;
        [BsonElement("pass")]
        public string Pass { get; set; } = null!;
        [BsonElement("mail")]
        public string Mail { get; set; } = null!;
        [BsonElement("country")]
        public string Country { get; set; } = null!;
    }
}