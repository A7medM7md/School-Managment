using School.Core.Features.Users.Commands.Models;
using School.Data.Entities.Identity;

namespace School.Core.Mapping.Users
{
    public partial class UserProfile
    {
        public void AddUserMapping()
        {
            CreateMap<AddUserCommand, AppUser>();
        }
    }
}
