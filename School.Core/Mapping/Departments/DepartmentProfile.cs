using AutoMapper;

namespace School.Core.Mapping.Departments
{
    public partial class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            GetDepartmentByIdMapping();
        }
    }
}
