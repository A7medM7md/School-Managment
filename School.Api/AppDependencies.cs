using School.Core;
using School.Infrastructure;
using School.Service;

namespace School.Api
{
    public static class AppDependencies
    {
        public static IServiceCollection AddAppDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureDependencies(configuration)
                .AddServiceDependencies()
                .AddCoreDependencies();


            return services;
        }
    }
}
