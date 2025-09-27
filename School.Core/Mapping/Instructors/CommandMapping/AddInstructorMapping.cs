using School.Core.Features.Instructors.Commands.Models;
using School.Data.Entities;

namespace School.Core.Mapping.Instructors
{
    public partial class InstructorProfile
    {
        public void AddInstructorMapping()
        {
            CreateMap<AddInstructorCommand, Instructor>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

        }

    }
}
