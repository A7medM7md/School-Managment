using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Users.Queries.Models;
using School.Core.Features.Users.Queries.Responses;
using School.Core.Resources;
using School.Core.Wrappers;
using School.Data.Entities.Identity;

namespace School.Core.Features.Users.Queries.Handlers
{
    public class UserQueryHandler : ResponseHandler,
                                    IRequestHandler<GetPaginatedUsersQuery, PaginatedResult<GetPaginatedUsersResponse>>,
                                    IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>
    {
        #region Fields

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion


        #region Constructors

        public UserQueryHandler(UserManager<AppUser> userManager,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _userManager = userManager;
            _mapper = mapper;
        }

        #endregion


        #region Handle Functions

        public async Task<PaginatedResult<GetPaginatedUsersResponse>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.
                AsNoTracking().
                Select(U => new GetPaginatedUsersResponse(
                    U.FullName,
                    U.Email,
                    U.Address,
                    U.Country
                ))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return users;
        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user is null) return NotFound<GetUserByIdResponse>(_localizer[SharedResourcesKeys.NotFound]);

            var mappedUser = _mapper.Map<GetUserByIdResponse>(user);

            return Success(mappedUser);
        }

        #endregion

    }
}
