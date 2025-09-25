using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Data.Entities.Views;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Abstracts.Functions;
using School.Infrastructure.Abstracts.Procedures;
using School.Infrastructure.Abstracts.Views;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories;
using School.Infrastructure.Repositories.Functions;
using School.Infrastructure.Repositories.Procedures;
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


            // App Entity Repositories
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Generic Repositories
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped(typeof(ISqlRepository), typeof(SqlRepository));

            // Sql View Repositories
            services.AddScoped(typeof(IViewRepository<DepartmentStudentsCountView>), typeof(DepartmentStudentsCountViewRepository));

            // Sql Stored Procedures Repositories
            services.AddScoped<IGetStudentsCountByDepartmentIdProcedureRepository, GetStudentsCountByDepartmentIdProcedureRepository>();

            // Sql Functions Repositories
            services.AddScoped<IInstructorFunctionsRepository, InstructorFunctionsRepository>();

            return services;
        }
    }
}
