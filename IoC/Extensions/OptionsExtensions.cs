using CrossCutting.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(option =>
            {
                option.ConnectionString = configuration.GetSection("MongoOptions:ConnectionString").Value;
                option.Database = configuration.GetSection("MongoOptions:Database").Value;
                option.Collection = configuration.GetSection("MongoOptions:Collection").Value;
            });

            return services;
        }
    }
}
