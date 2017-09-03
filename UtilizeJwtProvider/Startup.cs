using System;
using System.Text;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using UtilizeJwtProvider.IdentityServer;
using UtilizeJwtProvider.Repository;
using UtilizeJwtProvider.Services;


namespace UtilizeJwtProvider
{
    public class Startup
    {
        public Startup( IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            
          
            
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnectionString =
                $"Server={Configuration["Database:Host"]};" +
                $"database={Configuration["Database:Name"]};" +
                $"uid={Configuration["Database:Username"]};" +
                $"pwd={Configuration["Database:Password"]};";
            
            services.AddDbContext<EventDbContext>(options =>
                options.UseMySql(@dbConnectionString));
            services.AddTransient<IPasswordService, PkcsSha256PasswordService>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IAggregateFactory, AggregateFactory>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddMvc();


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
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Elasticsearch().WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri($"{Configuration["Logging:ElasticSearch"]}"))
                {
                    MinimumLogEventLevel = LogEventLevel.Verbose,
                    AutoRegisterTemplate = true,
                })
                .CreateLogger();
                       
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddSerilog();

            if (!env.IsDevelopment())
            {
                app.UseJwtBearerAuthentication(JwtBearerOptions(env.IsStaging()));
            }



            app.UseMvc();
            app.UseIdentityServer();
            
            
            

            using (var context = new EventDbContext())
            {
                context.Database.EnsureCreated();
            }

        }
        
        private static JwtBearerOptions JwtBearerOptions(bool isStaging)
        {
            var bearerOptions = new JwtBearerOptions
            {
                Audience = "Utilize API",
                Authority = "http://localhost:5000/",
                AutomaticAuthenticate = true,
            };

            if (isStaging)
            {
                bearerOptions.RequireHttpsMetadata = false;
            }
            return bearerOptions;
        }
    }
}