using Application.Command.Handlers;
using Application.Query.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Extensions
{
    public static class HandlerExtensions
    {
        public static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(StatusHandler).Assembly);
            services.AddMediatR(typeof(TransferHandler).Assembly);

            return services;
        }
    }
}
