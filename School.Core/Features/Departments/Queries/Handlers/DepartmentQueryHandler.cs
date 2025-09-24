using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Departments.Queries.Models;
using School.Core.Features.Departments.Queries.Responses;
using School.Core.Resources;
using School.Core.Wrappers;
using School.Data.Commons;
using School.Data.Entities;
using School.Service.Abstracts;
using System.Linq.Expressions;

namespace School.Core.Features.Departments.Queries.Handlers
{
    public class DepartmentQueryHandler : ResponseHandler,
                                        IRequestHandler<GetDepartmentByIdQuery, Response<GetDepartmentByIdResponse>>,
                                        IRequestHandler<GetDepartmentStudentsCountQuery, Response<List<GetDepartmentStudentsCountResponse>>>

    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion


        #region Constructors

        public DepartmentQueryHandler(IMapper mapper,
            IDepartmentService departmentService,
            IStudentService studentService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _mapper = mapper;
            _departmentService = departmentService;
            _studentService = studentService;
            _stringLocalizer = stringLocalizer;
        }


        #endregion


        #region Handle Functions

        public async Task<Response<GetDepartmentByIdResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            // Get Department Included Instructors, Subjects, Students Inside/BelongsTo It
            var department = await _departmentService.GetDepartmentByIdAsync(request.Id);

            if (department is null) return NotFound<GetDepartmentByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var mappedDepartment = _mapper.Map<GetDepartmentByIdResponse>(department);

            // Paginate Students List Inside Department

            Expression<Func<Student, StudentResponse>> expression = e => new StudentResponse(e.Id, e.GetLocalizedName());
            var studentsQueryable = _studentService.GetStudentsByDepartmentIdQueryable(request.Id);
            var studentPaginatedList = await studentsQueryable
                .Select(expression)
                .ToPaginatedListAsync(request.StudentPageNumber, request.StudentPageSize);

            mappedDepartment.Students = studentPaginatedList;

            return Success(mappedDepartment);
        }

        public async Task<Response<List<GetDepartmentStudentsCountResponse>>> Handle(GetDepartmentStudentsCountQuery request, CancellationToken cancellationToken)
        {
            var departmentStudentsCount = await _departmentService.GetDepartmentStudentsCount().ToListAsync();

            var result = _mapper.Map<List<GetDepartmentStudentsCountResponse>>(departmentStudentsCount);

            return Success(result);
        }


        #endregion


    }
}
