using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories;

namespace School.Infrastructure
{
    public static class InfrastructureModuleDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Connection To SQL Server
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();

            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            return services;
        }
    }
}
