using AutoMapper;
using EntityFrameworkCore.Testing.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Moq;
using School.Core.Features.Students.Queries.Handlers;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.Responses;
using School.Core.Mapping.Students;
using School.Core.Resources;
using School.Core.Wrappers;
using School.Data.Commons;
using School.Data.Entities;
using School.Data.Helpers.Enums;
using School.Service.Abstracts;
using School.xUnitTest.Core.Tests.TestModels;
using System.Globalization;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, MaxParallelThreads = 6)]
namespace School.xUnitTest.Core.Tests.Students.Queries
{
    public class StudentQueryHandlerTest
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly IMapper _mapper;
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;

        public StudentQueryHandlerTest()
        {
            _mockStudentService = new();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<StudentProfile>());
            _mapper = configuration.CreateMapper();
            _mockLocalizer = new();
        }

        [Fact(DisplayName = "Handle should return list of students successfully")]
        public async Task GetAllStudents_Should_Return_List_Of_Students()
        {
            // Arrange
            var query = new GetStudentsListQuery();

            var response = new List<Student>()
            {
                new() {Id=1, Address="Alex", DepartmentId=1, NameAr="أحمد", NameEn="Ahmed"}
            };

            _mockStudentService.Setup(s => s.GetStudentsListAsync()).ReturnsAsync(response);

            var handler = new StudentQueryHandler(
                _mockStudentService.Object,
                _mapper,
                _mockLocalizer.Object
            );

            Thread.CurrentThread.CurrentCulture = new CultureInfo("ar");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().BeOfType<Response<List<GetStudentsListResponse>>>();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNullOrEmpty();
            result.Data.Should().HaveCount(1);
            result.Data.First().Name.Should().Be("أحمد");
        }

        [Theory]
        //[InlineData(1)]
        [ClassData(typeof(PassDataUsingClassData))]
        public async Task GetStudentById_Should_Return_Student_With_StatusCode_200(int id)
        {
            // Arrange
            var query = new GetStudentByIdQuery(id);

            var dept1 = new Department()
            {
                Id = 1,
                NameAr = "هندسة البرمجيات",
                NameEn = "SE"
            };

            var students = new List<Student>()
            {
                new() {Id=1, Address="Alex", DepartmentId=1, NameAr="أحمد", NameEn="Ahmed", Department=dept1},
                new() {Id=2, Address="Cairo", DepartmentId=1, NameAr="محمد", NameEn="Mohamed", Department=dept1},
            };

            _mockStudentService.Setup(s => s.GetStudentByIdAsync(id)).ReturnsAsync(students.SingleOrDefault(s => s.Id == id));

            var handler = new StudentQueryHandler(
                _mockStudentService.Object,
                _mapper,
                _mockLocalizer.Object
            );

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be("Ahmed");
        }

        [Theory(DisplayName = "Handle should return 404 when student not found")]
        //[InlineData(3)]
        //[InlineData(2)]
        //[MemberData(nameof(GetStudentsIds))]
        [MemberData(nameof(PassDataUsingMemberData.GetStudentsIds), MemberType = typeof(PassDataUsingMemberData))]
        public async Task GetStudentById_Should_Return_NotFound_With_StatusCode_404(int id)
        {
            // Arrange
            var query = new GetStudentByIdQuery(id);

            var dept1 = new Department()
            {
                Id = 1,
                NameAr = "هندسة البرمجيات",
                NameEn = "SE"
            };

            var students = new List<Student>()
            {
                new() {Id=1, Address="Alex", DepartmentId=1, NameAr="أحمد", NameEn="Ahmed", Department=dept1},
                new() {Id=2, Address="Cairo", DepartmentId=1, NameAr="محمد", NameEn="Mohamed", Department=dept1},
            };

            _mockStudentService.Setup(s => s.GetStudentByIdAsync(id)).ReturnsAsync(students.SingleOrDefault(s => s.Id == id));

            var handler = new StudentQueryHandler(
                _mockStudentService.Object,
                _mapper,
                _mockLocalizer.Object
            );

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
        }

        [Theory]
        [InlineData(1, "Ahmed")]
        [InlineData(2, "Sara")]
        public async Task GetStudentById_Should_Return_Correct_Name(int id, string expectedName)
        {
            // Arrange
            var query = new GetStudentByIdQuery(id);

            var student = new Student { Id = id, NameEn = expectedName };
            _mockStudentService.Setup(s => s.GetStudentByIdAsync(id)).ReturnsAsync(student);

            var handler = new StudentQueryHandler(
                _mockStudentService.Object,
                _mapper,
                _mockLocalizer.Object
            );

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Name.Should().Be(expectedName);
        }


        [Fact(DisplayName = "Handle should return paginated list of students successfully")]
        public async Task GetStudents_Should_Return_Paginated_List_Of_Students()
        {
            // Arrange
            var query = new GetStudentsPaginatedListQuery()
            {
                PageNumber = 1,
                PageSize = 10,
                OrderBy = StudentOrderBy.StudentId,
                Search = "Ahmed"
            };

            var dept1 = new Department()
            {
                Id = 1,
                NameAr = "هندسة البرمجيات",
                NameEn = "SE"
            };

            var response = new AsyncEnumerable<Student>(new List<Student>
            {
                new() {Id=1, Address="Alex", DepartmentId=1, NameAr="أحمد", NameEn="Ahmed", Department = dept1}
            });

            _mockStudentService.Setup(s => s.GetFilteredStudentsQueryable(query.OrderBy, query.Search)).Returns(response.AsQueryable());

            var handler = new StudentQueryHandler(
                _mockStudentService.Object,
                _mapper,
                _mockLocalizer.Object
            );

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().BeOfType<PaginatedResult<GetStudentsPaginatedListResponse>>();
            result.Data.Should().BeOfType<List<GetStudentsPaginatedListResponse>>();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNullOrEmpty();
            result.Data.Should().HaveCount(1);
            result.Data.First().Name.Should().Be("Ahmed");
        }


        //[Fact]
        //public void Test1()
        //{
        //    Thread.Sleep(3000);
        //}

        //[Fact]
        //public void Test2()
        //{
        //    Thread.Sleep(5000);
        //}


        // Member Method
        public static IEnumerable<object[]> GetStudentsIds()
        {
            yield return new object[] { 1 };
            yield return new object[] { 2 };
            yield return new object[] { 3 };
            yield return new object[] { 0 };
            yield return new object[] { 100 };
            yield return new object[] { -5 };
        }
    }
}
