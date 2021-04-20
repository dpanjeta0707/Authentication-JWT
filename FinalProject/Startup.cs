using FinalProject.AuthorizationRequirements;
using FinalProject.Controllers;
using FinalProject.CustomPolicyProvider;
using FinalProject.Transformer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Our.Cookie";
                    config.LoginPath = "/Home/Authenticate";
                });

            services.AddAuthorization(config =>
           {
               // var defaultAuthBuilder = new AuthorizationPolicyBuilder();
               //var defaultAuthPolicy = defaultAuthBuilder
               //.RequireAuthenticatedUser()
               //.RequireClaim(ClaimTypes.DateOfBirth)
               //.Build();

               // config.DefaultPolicy = defaultAuthPolicy;

               config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));
               config.AddPolicy("Claim.DoB", policyBuilder =>
               {
                   policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
               });

           });

            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, SecurityLevelHandler>();
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

            services.AddControllersWithViews(config => {
                
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                    .RequireAuthenticatedUser()
                    .Build();

                //global authorization filter
                //config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
            });

            services.AddRazorPages()
                .AddRazorPagesOptions(config =>
                {
                    config.Conventions.AuthorizePage("/Razor/Secured");
                    config.Conventions.AuthorizePage("/Razor/Policy", "Admin");
                    config.Conventions.AuthorizeFolder("/RazorSecured");
                    config.Conventions.AllowAnonymousToPage("/RazorSecured/Anon");
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}