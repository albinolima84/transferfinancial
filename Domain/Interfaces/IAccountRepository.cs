using Domain.Models;
using RestSharp;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        IRestResponse VerifyAccount(string account);

        IRestResponse EffectiveTransfer(Transaction transaction);
    }
}
