using Domain.Models;
using Infra.Data.Dtos;

namespace Infra.Data.Extensions
{
    public static class TransferExtensions
    {
        public static TransferDto ToDto(this TransferFinancial transferFinancial) => 
            new TransferDto
            {
                TransactionId = transferFinancial.TransactionId,
                AccountOrigin = transferFinancial.AccountOrigin,
                AccountDestination = transferFinancial.AccountDestination,
                Value = transferFinancial.Value,
                Status = transferFinancial.Status.ToString()
            };
    }
}
