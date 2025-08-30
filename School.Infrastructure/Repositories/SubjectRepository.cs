using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;

namespace School.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepositoryAsync<Subject>, ISubjectRepository
    {

        #region Fields and Properties

        private readonly DbSet<Subject> subjects;

        #endregion

        #region Constructors
        public SubjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            subjects = dbContext.Set<Subject>();
        }

        #endregion


        #region Handle Functions 


        #endregion

    }
}
