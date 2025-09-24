using School.Core.Features.Departments.Queries.Responses;
using School.Data.Commons;
using School.Data.Entities.Views;

namespace School.Core.Mapping.Departments
{
    public partial class DepartmentProfile
    {
        public void GetDepartmentStudentsCountMapping()
        {
            CreateMap<DepartmentStudentsCountView, GetDepartmentStudentsCountResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalizedName()));
        }
    }
}
