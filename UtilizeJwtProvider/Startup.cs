using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
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
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IAggregateFactory, AggregateFactory>();
            services.AddTransient<IUserCache, UserCache>();
            services.AddTransient<ICacheWarmer, CacheWarmer>();
            
            services.AddMemoryCache();
            //services.AddMvc();
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();


            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "Utilize API";
                });
            
//            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
//                .AddIdentityServerAuthentication(options =>
//                {
//                    options.Authority = "http://localhost:5000/";
//                    options.ApiName = "Utilize API";
//                });
//            
            
            // Configure IdentityServer
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

           // services.AddMvc();
            services.AddLogging();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ICacheWarmer cacheWarmer)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddSerilog();

            app.UseAuthentication();
            app.UseMvc();
            app.UseIdentityServer();
            
            cacheWarmer.WarmCache();
           
        }

    }
}