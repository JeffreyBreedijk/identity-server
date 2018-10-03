using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Npgsql;
using Utilize.Identity.Provider.IdentityServer;
using Utilize.Identity.Provider.Options;
using Utilize.Identity.Provider.Repository;
using Utilize.Identity.Provider.Services;
using AuthDbContext = Utilize.Identity.Provider.DataSources.AuthDbContext;
using IPasswordService = Utilize.Identity.Provider.Services.IPasswordService;
using IPermissionSchemeService = Utilize.Identity.Provider.Services.IPermissionSchemeService;
using IUserService = Utilize.Identity.Provider.Services.IUserService;
using PermissionSchemeService = Utilize.Identity.Provider.Services.PermissionSchemeService;
using PkcsSha256PasswordService = Utilize.Identity.Provider.Services.PkcsSha256PasswordService;
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
            var dbConnectionString =
                new NpgsqlConnectionStringBuilder
                {
                    Host = "localhost",
                    Port = 26257,
                    Username = "utilize",
                    Database = "identity"
                }.ConnectionString;

            // Configure DB connection
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(@dbConnectionString));
            

            // Configure other services
            services.AddTransient<IPasswordService, PkcsSha256PasswordService>();
            
            // Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPermissionSchemeService, PermissionSchemeService>();
            

            services.Configure<ConfigurationOptions>(Configuration);
            services.AddTransient<IClientStore, ClientService>();
            services.AddTransient<IClientWriteStore, ClientService>();
            services.AddTransient<IResourceStore, ResourceService>();
            services.AddTransient<IIdentityServerRepository, MongoIdentityServerRepository>();

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            // Configure IdentityServer
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddClientStore<ClientService>()
                .AddResourceStore<ResourceService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            

            
            
            
            
            // services.AddMvc();
            services.AddLogging();
            //services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IIdentityServerRepository identityServerRepository, IOptions<ConfigurationOptions> optionsAccessor)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseMvc();
            app.UseIdentityServer();
            
            ConfigureMongoDriver2IgnoreExtraElements();
            Seed(identityServerRepository);
            
           

        }

        private static void Seed(IIdentityServerRepository identityServerRepository)
        {
            
            if (!identityServerRepository.CollectionExists<ApiResource>())
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

                    identityServerRepository.Add<ApiResource>(defaultResource);

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
