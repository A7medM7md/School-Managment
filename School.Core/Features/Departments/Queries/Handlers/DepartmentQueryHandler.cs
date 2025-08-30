using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Departments.Queries.Models;
using School.Core.Features.Departments.Queries.Responses;
using School.Core.Resources;
using School.Service.Abstracts;

namespace School.Core.Features.Departments.Queries.Handlers
{
    public class DepartmentQueryHandler : ResponseHandler,
                                        IRequestHandler<GetDepartmentByIdQuery, Response<GetDepartmentByIdResponse>>
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion


        #region Constructors

        public DepartmentQueryHandler(IMapper mapper,
            IDepartmentService departmentService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _mapper = mapper;
            _departmentService = departmentService;
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

            return Success(mappedDepartment);
        }


        #endregion


    }
}
