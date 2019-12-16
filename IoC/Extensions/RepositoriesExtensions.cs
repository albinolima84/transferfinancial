using Domain.Interfaces;
using Infra.Data.Connection;
using Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Extensions
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IConnect, Connect>();
            services.AddSingleton<ITransferRepository, TransferRepository>();

            return services;
        }
    }
}
