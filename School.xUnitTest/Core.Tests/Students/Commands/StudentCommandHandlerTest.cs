using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Features.Students.Commands.Handlers;
using School.Core.Features.Students.Commands.Models;
using School.Core.Mapping.Students;
using School.Core.Resources;
using School.Data.Entities;
using School.Service.Abstracts;

namespace School.xUnitTest.Core.Tests.Students.Commands
{
    public class StudentCommandHandlerTest
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly IMapper _mapper;
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;

        public StudentCommandHandlerTest()
        {
            _mockStudentService = new();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<StudentProfile>());
            _mapper = configuration.CreateMapper();
            _mockLocalizer = new();
        }

        [Fact]
        public async Task AddStudent_Should_Return_Status_Code_201()
        {
            // Arrange
            var command = new AddStudentCommand()
            {
                NameEn = "Ahmed",
                NameAr = "أحمد",
                DepartmentId = 1,
                Address = "Alex",
                Phone = "00000000"
            };

            var handler = new StudentCommandHandler(_mockStudentService.Object, _mapper,
                _mockLocalizer.Object);

            _mockStudentService.Setup(s => s.AddStudentAsync(It.IsAny<Student>())).ReturnsAsync("Succeeded");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Succeeded.Should().BeTrue();
            /// Verify
            _mockStudentService.Verify(s => s.AddStudentAsync(It.IsAny<Student>()), Times.Once, "Fail To Call");
        }

        [Fact]
        public async Task AddStudent_Should_Return_Status_Code_400()
        {
            // Arrange
            var command = new AddStudentCommand()
            {
                NameEn = "Ahmed",
                NameAr = "أحمد",
                DepartmentId = 1,
                Address = "Alex",
                Phone = "00000000"
            };

            var handler = new StudentCommandHandler(_mockStudentService.Object, _mapper,
                _mockLocalizer.Object);

            _mockStudentService.Setup(s => s.AddStudentAsync(It.IsAny<Student>())).ReturnsAsync("Failed");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Succeeded.Should().BeFalse();
            /// Verify
            _mockStudentService.Verify(s => s.AddStudentAsync(It.IsAny<Student>()), Times.Once, "Fail To Call");
        }

    }
}
