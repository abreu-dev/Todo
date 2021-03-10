using Todo.Application.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Todo.Api.Configurations
{
    public static class EmailServiceConfig
    {
        public static IServiceCollection AddEmailServiceConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var appSettingsSection = configuration.GetSection("AppEmailSettings");
            services.Configure<AppEmailSettings>(appSettingsSection);

            return services;
        }
    }
}
