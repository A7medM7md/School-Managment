using MediatR;
using School.Core.Bases;
using School.Core.Features.Departments.Queries.Responses;
using System.Text.Json.Serialization;

namespace School.Core.Features.Departments.Queries.Models
{
    public class GetDepartmentByIdQuery : IRequest<Response<GetDepartmentByIdResponse>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int StudentPageNumber { get; set; }
        public int StudentPageSize { get; set; }

        public GetDepartmentByIdQuery(int id, int studentPageNumber, int studentPageSize)
        {
            Id = id;
            StudentPageNumber = studentPageNumber;
            StudentPageSize = studentPageSize;
        }

    }
}
