using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.Responses;
using School.Core.Resources;
using School.Core.Wrappers;
using School.Data.Entities;
using School.Service.Abstracts;
using System.Linq.Expressions;

namespace School.Core.Features.Students.Queries.Handlers
{
    public class StudentQueryHandler : ResponseHandler,
                                        IRequestHandler<GetStudentsListQuery, Response<List<GetStudentsListResponse>>>,
                                        IRequestHandler<GetStudentByIdQuery, Response<GetSingleStudentResponse>>,
                                        IRequestHandler<GetStudentsPaginatedListQuery, PaginatedResult<GetStudentsPaginatedListResponse>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion

        #region Constructors
        public StudentQueryHandler(IStudentService studentService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        #endregion


        #region Handle Functions
        public async Task<Response<List<GetStudentsListResponse>>> Handle(GetStudentsListQuery request, CancellationToken cancellationToken)
        {
            var studentsList = await _studentService.GetStudentsListAsync();
            var studentsListMapper = _mapper.Map<List<GetStudentsListResponse>>(studentsList);
            return Success(studentsListMapper);
        }

        public async Task<Response<GetSingleStudentResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentService.GetStudentByIdAsync(request.Id);

            if (student is null) return NotFound<GetSingleStudentResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSingleStudentResponse>(student);

            return Success(result);
        }

        public async Task<PaginatedResult<GetStudentsPaginatedListResponse>> Handle(GetStudentsPaginatedListQuery request, CancellationToken cancellationToken)
        {
            // Expression For Taking Values From Student and Put Them Into GetStudentsPaginatedListResponse DTO [Just Expression, Not Executable Code]
            // Expression For Projection (Student → DTO)
            Expression<Func<Student, GetStudentsPaginatedListResponse>> expression = e => new GetStudentsPaginatedListResponse(e.Id, e.NameEn, e.Address, e.Department.NameEn);

            // Step 1: Get IQueryable<Student>
            //var students = _studentService.GetStudentsQueryable();

            // Step 2: Apply Filters [Search & OrderBy] and Using Step 1 Inside It .. So, No Need Of Step 1 Here
            var query = _studentService.GetFilteredStudentsQueryable(request.OrderBy, request.Search);

            // Step 3: Project To DTO 'IQueryable<GetStudentsPaginatedListResponse>' + Paginate
            var result = await query
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return result;
        }


        #endregion

    }
}
