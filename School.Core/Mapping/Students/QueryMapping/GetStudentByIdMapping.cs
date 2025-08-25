using School.Core.Features.Students.Queries.Responses;
using School.Data.Commons;
using School.Data.Entities;

namespace School.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void GetStudentByIdMapping()
        {
            CreateMap<Student, GetSingleStudentResponse>()
               .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.NameEn))
               //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalizedName()));
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => LocalizationHelper.GetLocalized(src.NameAr, src.NameEn)));
        }
    }
}
