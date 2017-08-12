using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UtilizeJwtProvider.IdentityServer;
using UtilizeJwtProvider.Models;
using UtilizeJwtProvider.Repository;
using UtilizeJwtProvider.Services;
using Resources = UtilizeJwtProvider.IdentityServer.Resources;

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
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<IClientStore, ClientStore>();

            
            // Add framework services.
            services.AddMvc();

            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddAspNetIdentity<ApplicationUser>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseIdentityServer();

            using (var context = new ApplicationDbContext())
            {
                context.Database.EnsureCreated();
            }
      
           

        }
    }
}