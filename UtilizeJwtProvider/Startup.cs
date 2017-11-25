using System;
using System.Text;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using UtilizeJwtProvider.DataSources;
using UtilizeJwtProvider.IdentityServer;
using UtilizeJwtProvider.Repository;
using UtilizeJwtProvider.Services;


namespace UtilizeJwtProvider
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        
        public Startup( IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

       

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnectionString =
                $"Server={Configuration["Database:Host"]};" +
                $"database={Configuration["Database:Name"]};" +
                $"uid={Configuration["Database:Username"]};" +
                $"pwd={Configuration["Database:Password"]};";
            
            // Configure DB connection
            services.AddDbContext<EventDbContext>(options =>
                options.UseMySql(@dbConnectionString));
            
            // Configure other services
            services.AddTransient<IPasswordService, PkcsSha256PasswordService>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IAggregateFactory, AggregateFactory>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            
            services.AddMvc();

            // Configure IdentityServer
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddSerilog();

            app.UseIdentityServerAuthentication(JwtBearerOptions(env.IsDevelopment()));

            app.UseMvc();
            app.UseIdentityServer();
            

        }
        
        private static IdentityServerAuthenticationOptions JwtBearerOptions(bool isStaging)
        {
            var bearerOptions = new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5000/",
                ApiName = "Utilize API",

                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            };

            if (isStaging)
            {
                bearerOptions.RequireHttpsMetadata = false;
            }
            return bearerOptions;
        }
    }
}