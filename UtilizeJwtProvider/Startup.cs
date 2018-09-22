
using System.Linq;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UtilizeJwtProvider.DataSources;
using UtilizeJwtProvider.IdentityServer;
using UtilizeJwtProvider.Repository;
using UtilizeJwtProvider.Services;


namespace UtilizeJwtProvider
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
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
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var dbConnectionString =
                $"Server={Configuration["Database:Host"]};" +
                $"database={Configuration["Database:Name"]};" +
                $"uid={Configuration["Database:Username"]};" +
                $"pwd={Configuration["Database:Password"]};";

            // Configure DB connection
            services.AddDbContext<UserDbContext>(options =>
                options.UseMySql(@dbConnectionString));

            // Configure other services
            services.AddTransient<IPasswordService, PkcsSha256PasswordService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, SqlUserRepository>();

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            // Configure IdentityServer
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseMySql(dbConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                }) 
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            // services.AddMvc();
            services.AddLogging();
            services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseMvc();
            app.UseIdentityServer();
        }

       
    }


}
