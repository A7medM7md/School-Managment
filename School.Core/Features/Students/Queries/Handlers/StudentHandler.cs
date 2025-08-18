using MediatR;
using School.Core.Features.Students.Queries.Models;
using School.Data.Entities;
using School.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Features.Students.Queries.Handlers
{
    public class StudentHandler : IRequestHandler<GetStudentsListQuery, List<Student>>
    {
        #region Fields
        private readonly IStudentService _studentService;

        #endregion

        #region Constructors
        public StudentHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        #endregion

        #region Handle Functions
        public async Task<List<Student>> Handle(GetStudentsListQuery request, CancellationToken cancellationToken)
        {
            return await _studentService.GetStudentsListAsync();
        }

        #endregion

    }
}
