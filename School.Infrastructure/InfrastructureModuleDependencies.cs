using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Data.Entities.Views;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Abstracts.Views;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories;
using School.Infrastructure.Repositories.Views;

namespace School.Infrastructure
{
    public static class InfrastructureModuleDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEncryptionProvider>(provider =>
                new GenerateEncryptionProvider(configuration["Encryption:SecretKey"]));

            // Connection To SQL Server
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });


            // Entity Repositories
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            // View Repositories
            services.AddScoped(typeof(IViewRepository<DepartmentStudentsCountView>), typeof(DepartmentStudentsCountViewRepository));

            return services;
        }
    }
}
