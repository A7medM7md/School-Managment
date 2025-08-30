using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;

namespace School.Infrastructure.Repositories
{
    public class InstructorRepository : GenericRepositoryAsync<Instructor>, IInstructorRepository
    {

        #region Fields and Properties

        private readonly DbSet<Instructor> instructors;

        #endregion

        #region Constructors
        public InstructorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            instructors = dbContext.Set<Instructor>();
        }

        #endregion


        #region Handle Functions 


        #endregion

    }
}
