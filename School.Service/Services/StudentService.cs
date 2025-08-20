using Microsoft.EntityFrameworkCore;
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

        #region Handle Functions
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _studentRepository.GetStudentsListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            //return await _studentRepository.GetByIdAsync(id);
            return await _studentRepository.GetTableNoTracking()
                .Include(S => S.Department)
                .Where(S => S.StudID.Equals(id))
                .FirstOrDefaultAsync(); 
        }

        public async Task<string> AddStudentAsync(Student student)
        {
            // Check If Student Name Exists Or Not [This Mainly Done With Fluent Validation, Not Here]
            var isExists = _studentRepository.GetTableNoTracking().Where(S => S.Name.Equals(student.Name)).FirstOrDefault() is not null;

            if (isExists) return "Exists";

            await _studentRepository.AddAsync(student);
            return "Succeeded";
        }


        #endregion

    }
}
