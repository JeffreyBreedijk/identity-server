using System;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Utilize.Identity.Provider.Options;
using Utilize.Identity.Provider.Repository;
using Utilize.Identity.Provider.Services;

namespace Utilize.Identity.Provider
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = Generic.Configuration.BuildConfiguration();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.Configure<ConfigurationOptions>(Configuration);
            services.AddTransient<IClientStore, ClientService>();
            services.AddTransient<IClientWriteStore, ClientService>();
            services.AddTransient<IResourceStore, ResourceService>();
            services.AddTransient<IReferenceTokenStore, ReferenceTokenStore>();
            services.AddTransient<IIdentityServerRepository, MongoIdentityServerRepository>();

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            // Configure IdentityServer
            services.AddIdentityServer()
//                .AddResourceStore<ResourceService>()
              //  .AddClientStore<ClientService>()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiResources(Config.GetApiResources());

            

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
 
                    options.ApiName = "api1";
                    options.ApiSecret = "secret";
                });
          
            
            services.AddMvc();
            services.AddLogging();
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = "localhost";
                option.InstanceName = "master";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseMvc();
            app.UseIdentityServer();
            
            
            ConfigureMongoDriver2IgnoreExtraElements();
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
