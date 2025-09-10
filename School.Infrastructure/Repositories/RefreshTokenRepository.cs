using School.Data.Entities.Identity;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;

namespace School.Infrastructure.Repositories
{
    internal class RefreshTokenRepository : GenericRepositoryAsync<RefreshToken>, IRefreshTokenRepository
    {
        #region Fields

        #endregion


        #region Constructors

        public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Methods

        #endregion

    }
}
