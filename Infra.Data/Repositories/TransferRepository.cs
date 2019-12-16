using CrossCutting.Options;
using Domain.Interfaces;
using Domain.Models;
using Infra.Data.Connection;
using Infra.Data.Dtos;
using Infra.Data.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Data.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private IMongoCollection<TransferDto> _mongoCollection;
        private readonly IConnect _connect;
        private readonly string _database;
        private readonly string _collection;

        public TransferRepository(IConnect connect, IOptions<MongoOptions> config)
        {
            _database = config?.Value?.Database;
            _collection = config?.Value?.Collection;
            SetConnectAndCollection(connect);
            _connect = connect;
        }

        internal void SetConnectAndCollection(IConnect connect) => _mongoCollection = connect.Collection<TransferDto>(_collection, _database);

        public async Task<string> GetTransfer(string transactionId)
        {
            TransferDto transferFinancial = null;

            var filter = Builders<TransferDto>.Filter.Eq("transactionId", transactionId);

            using (IAsyncCursor<TransferDto> cursor = await _mongoCollection.FindAsync<TransferDto>(filter))
            {
                await cursor.MoveNextAsync();
                if (cursor.Current.Any())
                {
                    transferFinancial = cursor.Current.FirstOrDefault();
                }
            }

            return transferFinancial?.Status;
        }

        public async Task<string> Transfer(TransferFinancial transferFinancial)
        {
            var transferDto = transferFinancial.ToDto();

            await _mongoCollection.InsertOneAsync(transferDto);

            return transferDto.TransactionId.ToString();
        }
    }
}
