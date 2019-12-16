using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.Data.Dtos
{
    [BsonIgnoreExtraElements]
    public class TransferDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("transactionId")]
        public string TransactionId { get; set; }

        [BsonElement("accountOrigin")]
        public string AccountOrigin { get; set; }

        [BsonElement("accountDestination")]
        public string AccountDestination { get; set; }

        [BsonElement("value")]
        public decimal Value { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }
    }
}
