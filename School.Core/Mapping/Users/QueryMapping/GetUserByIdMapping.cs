using School.Core.Features.Users.Queries.Responses;
using School.Data.Entities.Identity;

namespace School.Core.Mapping.Users
{
    public partial class UserProfile
    {
        public void GetUserByIdMapping()
        {
            CreateMap<AppUser, GetUserByIdResponse>();
        }
    }
}
