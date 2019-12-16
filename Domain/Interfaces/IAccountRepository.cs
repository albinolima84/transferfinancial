using RestSharp;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        IRestResponse VerifyAccount(string account);
    }
}
