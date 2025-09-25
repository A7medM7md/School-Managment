using School.Core.Features.Instructors.Queries.Responses;
using School.Data.Commons;
using School.Data.Entities;

namespace School.Core.Mapping.Instructors
{
    public partial class InstructorProfile
    {
        public void GetInstructorsDataMapping()
        {
            CreateMap<Instructor, GetInstructorsDataResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalizedName()));
        }
    }
}
