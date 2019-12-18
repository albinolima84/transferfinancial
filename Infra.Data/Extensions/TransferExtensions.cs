using Domain.Enums;
using Domain.Models;
using Infra.Data.Dtos;
using System;

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
                Status = transferFinancial.Status.ToString(),
                ErrorMessage = transferFinancial .ErrorMessage
            };

        public static TransferFinancial ToDomain(this TransferDto transferDto)
        {
            StatusEnum status = (StatusEnum)Enum.Parse(typeof(StatusEnum), transferDto.Status, true);

            return new TransferFinancial(transferDto.TransactionId, transferDto.AccountOrigin, transferDto.AccountDestination, transferDto.Value, status, transferDto.ErrorMessage);
        }
    }
}
