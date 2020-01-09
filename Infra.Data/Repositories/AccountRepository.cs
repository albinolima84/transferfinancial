using CrossCutting.Options;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using System;

namespace Infra.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _endpointVerifyAccount;
        private readonly string _endpointTransferFinancial;
        private readonly IRestClient _client;

        public AccountRepository(IOptions<AccountOptions> options, IRestClient client)
        {
            _endpointVerifyAccount = options.Value.UrlVerifyAccount;
            _endpointTransferFinancial = options.Value.UrlTransfer;
            _client = client;
        }

        public IRestResponse EffectiveTransfer(Transaction transaction)
        {
            try
            {
                var request = new RestRequest(_endpointTransferFinancial, Method.POST);
                request.AddJsonBody(transaction);

                return _client.Execute(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IRestResponse VerifyAccount(string account)
        {
            try
            {
                var request = new RestRequest(_endpointVerifyAccount, Method.GET);
                request.AddUrlSegment("accountNumber", account);

                return _client.Execute(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
