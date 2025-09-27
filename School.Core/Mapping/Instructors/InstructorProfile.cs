using AutoMapper;

namespace School.Core.Mapping.Instructors
{
    public partial class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            GetInstructorsDataMapping();
            AddInstructorMapping();
        }
    }
}
