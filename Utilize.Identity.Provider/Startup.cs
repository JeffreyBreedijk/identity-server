using System.IO;
using FluentValidation.AspNetCore;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Utilize.Identity.Provider.Options;
using Utilize.Identity.Provider.Repository;
using Utilize.Identity.Provider.Repository.Clients;
using Utilize.Identity.Provider.Repository.Mongo;
using Utilize.Identity.Provider.Services;

namespace Utilize.Identity.Provider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile("appsettings.yaml")
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConfigurationOptions>(Configuration);
            services.AddTransient<IClientStore, MongoClientRepository>();
            services.AddTransient<IClientWriteStore, MongoClientRepository>();
            services.AddTransient<IResourceStore, ResourceRepository>();
            services.AddTransient<IResourceWriteStore, ResourceRepository>();
            services.AddTransient<IReferenceTokenStore, ReferenceTokenStore>();

            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddAuthorization();

            services.AddIdentityServer()
                .AddResourceStore<ResourceRepository>()
                .AddClientStore<MongoClientRepository>()
                .AddDeveloperSigningCredential();

            services.AddLogging();
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = "localhost";
                option.InstanceName = "master";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseIdentityServer();

            ConfigureMongoDriver2IgnoreExtraElements();
        }

        private static void ConfigureMongoDriver2IgnoreExtraElements()
        {
            BsonClassMap.RegisterClassMap<Client>(cm =>
            {
                cm.AutoMap();
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