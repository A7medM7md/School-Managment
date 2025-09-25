using School.Core.Features.Departments.Queries.Models;
using School.Core.Features.Departments.Queries.Responses;
using School.Data.Commons;
using School.Data.Entities.Procedures;

namespace School.Core.Mapping.Departments
{
    public partial class DepartmentProfile
    {
        public void GetStudentsCountByDepartmentIdMapping()
        {
            CreateMap<GetStudentsCountByDepartmentIdProcedure, GetStudentsCountByDepartmentIdResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalizedName()));


            CreateMap<GetStudentsCountByDepartmentIdQuery, GetStudentsCountByDepartmentIdProcedureParams>();

        }
    }
}
