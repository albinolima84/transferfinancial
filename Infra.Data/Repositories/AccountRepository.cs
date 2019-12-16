using CrossCutting.Options;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using System;

namespace Infra.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _endpoint;
        private readonly IRestClient _client;

        public AccountRepository(IOptions<AccountOptions> options, IRestClient client)
        {
            _endpoint = options.Value.UrlAccount;
            _client = client;
        }

        public IRestResponse VerifyAccount(string account)
        {
            try
            {
                var request = new RestRequest(_endpoint, Method.GET);
                request.AddUrlSegment("accountNumber", account);

                return _client.Execute(request);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
