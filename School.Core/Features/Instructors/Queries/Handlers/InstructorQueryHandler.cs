using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Instructors.Queries.Models;
using School.Core.Features.Instructors.Queries.Responses;
using School.Core.Resources;
using School.Data.Commons;
using School.Service.Abstracts;

namespace School.Core.Features.Instructors.Queries.Handlers
{
    internal class InstructorQueryHandler : ResponseHandler,
                                                IRequestHandler<GetInstructorsSalarySummationQuery, Response<decimal>>,
                                                IRequestHandler<GetInstructorsDataQuery, Response<List<GetInstructorsDataResponse>>>
    {
        private readonly IInstructorService _instructorService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public InstructorQueryHandler(IInstructorService instructorService,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _instructorService = instructorService;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Response<decimal>> Handle(GetInstructorsSalarySummationQuery request, CancellationToken cancellationToken)
        {
            var instructorsTotalSalary = await _instructorService.GetTotalSalary();

            return Success(instructorsTotalSalary);
        }

        public async Task<Response<List<GetInstructorsDataResponse>>> Handle(GetInstructorsDataQuery request, CancellationToken cancellationToken)
        {
            var instructors = await _instructorService.GetAllInstructors();

            var mappedInstructors = _mapper.Map<List<GetInstructorsDataResponse>>(instructors);

            return Success(mappedInstructors);
        }
    }
}
