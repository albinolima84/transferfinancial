using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransferRepository
    {
        Task<string> Transfer(TransferFinancial transferFinancial);

        Task<TransferFinancial> GetTransfer(string transactionId);

        Task UpdateStatus(TransferFinancial transferFinancial);
    }
}
