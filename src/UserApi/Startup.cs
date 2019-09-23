using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using UserApi.Policies;
using UserApi.Security;
using UserData;
using UserData.AppContext;

namespace UserApi
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
            services.AddDbContext<UserDataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("User")));
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                // .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<UserDataContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.User.RequireUniqueEmail = true;
            });

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            // services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permissions.Geral.ReadAdmin", builder =>
                {
                    builder.AddRequirements(new PermissionRequirement("Permissions.Geral.Read"));
                    builder.RequireRole("Admin");
                });


                // The rest omitted for brevity.
            });

            services.AddScoped<AccessManager>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);


            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            // Aciona a extensão que irá configurar o uso de
            // autenticação e autorização via tokens
            services.AddJwtSecurity(
                signingConfigurations, tokenConfigurations);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var sp = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var ctx = sp.ServiceProvider.GetRequiredService<UserDataContext>())
                {
                    ctx.Database.Migrate();
                }
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

    }
}
