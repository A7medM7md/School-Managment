using AutoMapper;

namespace School.Core.Mapping.Users
{
    public partial class UserProfile : Profile
    {
        public UserProfile()
        {
            GetUserByIdMapping();
            AddUserMapping();
            EditUserMapping();
        }
    }
}
