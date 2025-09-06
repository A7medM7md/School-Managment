using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using School.Core;
using School.Data.Entities.Identity;
using School.Infrastructure;
using School.Infrastructure.Context;
using School.Service;
using System.Globalization;

namespace School.Api
{
    public static class AppDependencies
    {
        public static IServiceCollection AddAppDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureDependencies(configuration)
                .AddServiceDependencies()
                .AddCoreDependencies();

            #region Configure Localization

            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-DE"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("ar-EG")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });


            #endregion


            // Add Identity With Configurations
            services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                // lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // signIn settings
                options.SignIn.RequireConfirmedEmail = false;

                // user settings
                options.User.RequireUniqueEmail = true;

                // password settings
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            return services;
        }
    }
}
