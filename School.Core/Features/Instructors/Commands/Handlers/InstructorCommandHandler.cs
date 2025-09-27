using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Instructors.Commands.Models;
using School.Core.Resources;
using School.Data.Commons;
using School.Data.Entities;
using School.Service.Abstracts;

namespace School.Core.Features.Instructors.Commands.Handlers
{
    public class InstructorCommandHandler : ResponseHandler,
                                        IRequestHandler<AddInstructorCommand, Response<string>>
    {
        #region Fields
        private readonly IInstructorService _instructorService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion


        #region Constructors

        public InstructorCommandHandler(IInstructorService instructorService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _instructorService = instructorService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        #endregion

        #region Handle Functions

        public async Task<Response<string>> Handle(AddInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructor = _mapper.Map<Instructor>(request);

            var result = await _instructorService.AddInstructorAsync(instructor, request.Image);

            if (result == "Failed")
                return BadRequest<string>("Failed to add instructor");

            return Success("Instructor added successfully");
        }


        #endregion
    }
}
