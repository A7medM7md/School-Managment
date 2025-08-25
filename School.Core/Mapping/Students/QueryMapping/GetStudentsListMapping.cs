using School.Core.Features.Students.Queries.Responses;
using School.Data.Commons;
using School.Data.Entities;

namespace School.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void GetStudentsListMapping()
        {
            CreateMap<Student, GetStudentsListResponse>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.NameEn))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalizedName()));
        }

    }
}
