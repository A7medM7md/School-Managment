using School.Core.Features.Users.Commands.Models;
using School.Data.Entities.Identity;

namespace School.Core.Mapping.Users
{
    public partial class UserProfile
    {
        public void EditUserMapping()
        {
            CreateMap<EditUserCommand, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
