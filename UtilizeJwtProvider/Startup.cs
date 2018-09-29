
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
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
            services.AddDbContext<AuthDbContext>(options =>
                options.UseMySql(@dbConnectionString));
            

            // Configure other services
            services.AddTransient<IPasswordService, PkcsSha256PasswordService>();
            
            // Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IPermissionSchemeService, PermissionSchemeService>();
            
            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<IPermissionSchemeRepository, PermissionSchemeRepository>();

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
            //services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            ConfigurationDbContext configurationDbContext)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseMvc();
            app.UseIdentityServer();
            
            
            SetDefaultApiResource(configurationDbContext);
        }

        private static void SetDefaultApiResource(ConfigurationDbContext configurationDbContext)
        {
            var scope = new Scope()
            {
                Name = "default",
                DisplayName = "default",
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.FamilyName,
                    JwtClaimTypes.GivenName,
                    "debtor_id",
                    "permissions"
                }
            };
            
            var resource = configurationDbContext.ApiResources.AsNoTracking().FirstOrDefault(r => r.Name.Equals("Default Resource"));
            if (resource != null) return;
            var defaultResource = new ApiResource
            {
                Name = "Default Resource",
                Scopes = new List<Scope>()
                {
                    new Scope()
                    {
                        Name = "default",
                        DisplayName = "default",
                        UserClaims =
                        {
                            JwtClaimTypes.Name,
                            JwtClaimTypes.Email,
                            JwtClaimTypes.FamilyName,
                            JwtClaimTypes.GivenName,
                            "debtor_id",
                            "permissions"
                        }
                    }
                }
            };

            configurationDbContext.ApiResources.Add(defaultResource.ToEntity());
            configurationDbContext.SaveChanges();









        }
    }


}
