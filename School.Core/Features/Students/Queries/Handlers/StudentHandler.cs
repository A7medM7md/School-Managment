using AutoMapper;
using MediatR;
using School.Core.Bases;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.Responses;
using School.Data.Entities;
using School.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Features.Students.Queries.Handlers
{
    public class StudentHandler : ResponseHandler, IRequestHandler<GetStudentsListQuery, Response<List<GetStudentsListResponse>>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors
        public StudentHandler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        #endregion


        #region Handle Functions
        public async Task<Response<List<GetStudentsListResponse>>> Handle(GetStudentsListQuery request, CancellationToken cancellationToken)
        {
            var studentsList = await _studentService.GetStudentsListAsync();
            var studentsListMapper = _mapper.Map<List<GetStudentsListResponse>>(studentsList);
            return Success(studentsListMapper);
        }



        #endregion

    }
}
