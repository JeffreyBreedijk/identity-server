using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilize.Identity.Manager.Authorization;
using Utilize.Identity.Shared.DataSources;
using Utilize.Identity.Shared.Repository;
using Utilize.Identity.Shared.Services;

namespace Utilize.Identity.Manager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {         
            
            var dbConnectionString =
                $"Server={Configuration["Database:Host"]};" +
                $"database={Configuration["Database:Name"]};" +
                $"uid={Configuration["Database:Username"]};" +
                $"pwd={Configuration["Database:Password"]};";
            
            // Configure DB connection
            services.AddDbContext<AuthDbContext>(options =>
                options.UseMySql(@dbConnectionString));
            
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<IdentityServerDbContext>(options =>
                options.UseMySql(@dbConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            
            // Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IPermissionSchemeService, PermissionSchemeService>();
            
            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<IPermissionSchemeRepository, PermissionSchemeRepository>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = false;
                    // base-address of your identityserver
                    options.Authority = "http://localhost:5000";

                    // name of the API resource
                    options.ApiName = "JB";
                    
                    
                });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasPermission", policy => policy.Requirements.Add(new PermissionRequirement("testpermission")));
             
            });
            
         
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
        
        
        
    }
}