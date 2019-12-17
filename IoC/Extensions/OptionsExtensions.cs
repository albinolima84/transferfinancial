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

            services.Configure<MessagingConfigurationOptions>(option =>
            {
                option.EndPoint = configuration.GetSection("MessagingConfigurationOptions:ConnectionString").Value;
                option.Queue = configuration.GetSection("MessagingConfigurationOptions:Queue").Value;
                option.AccessKey = configuration.GetSection("MessagingConfigurationOptions:AccessKey").Value;
                option.AccessKeyName = configuration.GetSection("MessagingConfigurationOptions:AccessKeyName").Value;
            });

            return services;
        }
    }
}
