using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Utilize.Identity.Provider.IdentityServer;
using Utilize.Identity.Provider.Options;
using Utilize.Identity.Provider.Repository;
using Utilize.Identity.Provider.Services;
using Utilize.Identity.Shared.DataSources;
using Utilize.Identity.Shared.Repository;
using Utilize.Identity.Shared.Services;
using AuthDbContext = Utilize.Identity.Provider.DataSources.AuthDbContext;
using IPasswordService = Utilize.Identity.Provider.Services.IPasswordService;
using IPermissionSchemeRepository = Utilize.Identity.Provider.Repository.IPermissionSchemeRepository;
using IPermissionSchemeService = Utilize.Identity.Provider.Services.IPermissionSchemeService;
using ITenantRepository = Utilize.Identity.Provider.Repository.ITenantRepository;
using ITenantService = Utilize.Identity.Provider.Services.ITenantService;
using IUserRepository = Utilize.Identity.Provider.Repository.IUserRepository;
using IUserService = Utilize.Identity.Provider.Services.IUserService;
using PermissionSchemeRepository = Utilize.Identity.Provider.Repository.PermissionSchemeRepository;
using PermissionSchemeService = Utilize.Identity.Provider.Services.PermissionSchemeService;
using PkcsSha256PasswordService = Utilize.Identity.Provider.Services.PkcsSha256PasswordService;
using TenantRepository = Utilize.Identity.Provider.Repository.TenantRepository;
using TenantService = Utilize.Identity.Provider.Services.TenantService;
using UserRepository = Utilize.Identity.Provider.Repository.UserRepository;
using UserService = Utilize.Identity.Provider.Services.UserService;

namespace Utilize.Identity.Provider
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

            services.Configure<ConfigurationOptions>(Configuration);
            services.AddTransient<IClientStore, CustomClientStore>();
            services.AddTransient<IResourceStore, CustomResourceStore>();
            services.AddTransient<IRepository, MongoRepository>();

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            // Configure IdentityServer
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddClientStore<CustomClientStore>()
                .AddResourceStore<CustomResourceStore>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            

            
            
            
            
            // services.AddMvc();
            services.AddLogging();
            //services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IRepository repository)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseMvc();
            app.UseIdentityServer();
            
            ConfigureMongoDriver2IgnoreExtraElements();
            Seed(repository);
        }

        private static void Seed(IRepository repository)
        {
            
            if (!repository.CollectionExists<ApiResource>())
            {
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

                    repository.Add<ApiResource>(defaultResource);

            }
        }

        
        private static void ConfigureMongoDriver2IgnoreExtraElements()
        {
          
            BsonClassMap.RegisterClassMap<Client>(cm =>
            {
               
                cm.AutoMap();
                var t = cm.GetMemberMap(x => x.ClientId).SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.SetIdMember(cm.GetMemberMap(x => x.ClientId).SetIdGenerator(StringObjectIdGenerator.Instance));
                cm.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<IdentityResource>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<ApiResource>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<PersistedGrant>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });



        }
    }


}
