using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Service.Services
{
    public class StudentService : IStudentService
    {

        #region Fields
        private readonly IStudentRepository _studentRepository;

        #endregion

        #region Constructors
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        #endregion

        #region Handles Functions
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _studentRepository.GetStudentsListAsync();
        }

        #endregion

    }
}
