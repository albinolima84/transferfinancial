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

            services.Configure<AccountOptions>(option =>
            {
                option.UrlAccount = configuration.GetSection("AccountOptions:urlAccount").Value;
            });

            services.Configure<RabbitOptions>(option =>
            {
                option.UserName = configuration.GetSection("RabbitOptions:UserName").Value;
                option.Password = configuration.GetSection("RabbitOptions:Password").Value;
                option.HostName = configuration.GetSection("RabbitOptions:HostName").Value;
                option.VHost = configuration.GetSection("RabbitOptions:VHost").Value;
                option.Port = int.Parse(configuration.GetSection("RabbitOptions:Port").Value);
            });

            services.Configure<MessagingConfigurationOptions>(option =>
            {
                option.Exchange = configuration.GetSection("MessagingConfigurationOptions:Exchange").Value;
                option.RoutingKey = configuration.GetSection("MessagingConfigurationOptions:RoutingKey").Value;
                option.ExchangeType = configuration.GetSection("MessagingConfigurationOptions:Queue").Value;
            });

            return services;
        }
    }
}
