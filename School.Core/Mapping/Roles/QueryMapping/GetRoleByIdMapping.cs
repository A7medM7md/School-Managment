using Microsoft.AspNetCore.Identity;
using School.Core.Features.Authorization.Queries.Responses;

namespace School.Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        public void GetRoleByIdMapping()
        {
            CreateMap<IdentityRole<int>, GetRoleByIdResponse>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
