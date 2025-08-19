using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {

        #region Fields
        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Constructors
        public StudentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Handles Functions
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _dbContext.Students.Include(S => S.Department).ToListAsync();
        }

        #endregion

    }
}
