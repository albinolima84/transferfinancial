using MongoDB.Driver;

namespace Infra.Data.Connection
{
    public interface IConnect
    {
        IMongoCollection<T> Collection<T>(string collectionName, string database);
    }
}
