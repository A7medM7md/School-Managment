using AutoMapper;
using MediatR;
using School.Core.Bases;
using School.Core.Features.Students.Commands.Models;
using School.Data.Entities;
using School.Service.Abstracts;

namespace School.Core.Features.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler,
                                        IRequestHandler<AddStudentCommand, Response<string>>,
                                        IRequestHandler<EditStudentCommand, Response<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors
        public StudentCommandHandler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var student = _mapper.Map<Student>(request);

            var result = await _studentService.AddStudentAsync(student);

            if (result == "Succeeded")
                return Created($"Student {student.Name} Was Added Successfully");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            // It's Better To Make Endpoint That Not Uses Include and Use It Here
            var isExists = await _studentService.GetStudentByIdAsync(request.Id) is not null;
            if (!isExists) return NotFound<string>("Student is not exists.");

            var student = _mapper.Map<Student>(request);

            var result = await _studentService.UpdateStudentAsync(student);
            if (result == "Succeeded")
                return Success($"Student With Id:{student.StudID} Was Updated Successfully");
            else
                return BadRequest<string>();
        }

        #endregion
    }
}
