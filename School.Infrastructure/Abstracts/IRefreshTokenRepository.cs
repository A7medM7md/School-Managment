using School.Data.Entities.Identity;
using School.Infrastructure.Bases;

namespace School.Infrastructure.Abstracts
{
    public interface IRefreshTokenRepository : IGenericRepositoryAsync<RefreshToken>
    {

    }
}
