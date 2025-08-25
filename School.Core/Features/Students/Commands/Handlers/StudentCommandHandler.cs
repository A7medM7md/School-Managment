using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Students.Commands.Models;
using School.Core.Resources;
using School.Data.Entities;
using School.Service.Abstracts;
using School.Service.Enums;

namespace School.Core.Features.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler,
                                        IRequestHandler<AddStudentCommand, Response<string>>,
                                        IRequestHandler<EditStudentCommand, Response<string>>,
                                        IRequestHandler<DeleteStudentCommand, Response<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion

        #region Constructors
        public StudentCommandHandler(IStudentService studentService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer
            ) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var student = _mapper.Map<Student>(request);

            var result = await _studentService.AddStudentAsync(student);

            if (result == "Succeeded")
                return Created($"Student {student.NameEn} Was Added Successfully");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            // It's Better To Make Endpoint That Not Uses Include and Use It Here
            var student = await _studentService.GetStudentByIdWithoutDepartmentAsync(request.Id);
            if (student is null) return NotFound<string>("Student is not exists.");

            //var student = _mapper.Map<Student>(request); // Throw EF Error [FindAsync Returns Tracked Instance and Mapping Return a New Instance With Same Id That Will Make Conflict {DbContext Has 2 Instances For The Same Obj With Same Id !!}]
            // Edit Values Come From request Only..
            _mapper.Map(request, student); // Here Will Update On The Same Tracked Object

            var result = await _studentService.UpdateStudentAsync(student);
            if (result == "Succeeded")
                return Success($"Student With Id:{student.Id} Was Updated Successfully");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var result = await _studentService.DeleteStudentAsync(request.Id);

            return result switch
            {
                DeleteStudentResult.NotFound => NotFound<string>($"Student with Id:{request.Id} doesn't exist."),
                DeleteStudentResult.Failed => BadRequest<string>($"Failed to delete Student with Id:{request.Id}."),
                DeleteStudentResult.Success => Deleted<string>($"Student with Id:{request.Id} was deleted successfully"),
                _ => BadRequest<string>("Unexpected error occurred.")
            };
        }

        #endregion
    }
}
