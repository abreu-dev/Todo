using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Todo.Api.Configurations;
using Todo.Api.Middlewares;
using Todo.Infra.CrossCutting.IoC;
using Todo.Infra.Data;

namespace Todo.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // WebAPI Config
            services.AddControllers();

            // Setting DbContexts
            services.AddDatabaseConfiguration(Configuration);

            // ASP.NET Identity Settings & JWT
            services.AddIdentityConfiguration(Configuration);

            // AutoMapper Settings
            services.AddAutoMapperConfiguration();

            // Swagger Config
            services.AddSwaggerConfiguration();

            // Adding MediatR for Commands and Notifications
            services.AddMediatR(typeof(Startup));

            // Email Service Config
            services.AddEmailServiceConfiguration(Configuration);

            // .NET Native DI Abstraction
            services.Register();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TodoContext todoContext)
        {
            if (env.IsProduction())
            {
                todoContext.Database.Migrate();
            }

            app.UseCors(configurePolicy =>
            {
                configurePolicy.AllowAnyOrigin();
                configurePolicy.AllowAnyMethod();
                configurePolicy.AllowAnyHeader();
            });

            app.UseSwaggerSetup();

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
