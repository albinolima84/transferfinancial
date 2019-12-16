using Domain.Interfaces;
using Infra.Data.Connection;
using Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace IoC.Extensions
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IConnect, Connect>();
            services.AddSingleton<ITransferRepository, TransferRepository>();
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IRestClient, RestClient>();

            return services;
        }
    }
}
