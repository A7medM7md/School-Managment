using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Authorization.Queries.Models;
using School.Core.Features.Authorization.Queries.Responses;
using School.Core.Resources;
using School.Data.Commons;
using School.Service.Abstracts;

namespace School.Core.Features.Authorization.Queries.Handlers
{
    public class RoleQueryHandler : ResponseHandler,
                                        IRequestHandler<GetRolesQuery, Response<IReadOnlyList<GetRolesResponse>>>,
                                        IRequestHandler<GetRoleByIdQuery, Response<GetRoleByIdResponse>>,
                                        IRequestHandler<GetRolesForUserQuery, Response<GetRolesForUserResponse>>
    {
        #region Fields

        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion

        #region Constructors

        public RoleQueryHandler(IAuthorizationService authorizationService,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }
        #endregion

        public async Task<Response<IReadOnlyList<GetRolesResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRolesAsync(cancellationToken);

            var mappedRoles = _mapper.Map<IReadOnlyList<GetRolesResponse>>(roles);

            return Success(mappedRoles);
        }

        public async Task<Response<GetRoleByIdResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleByIdAsync(request.Id);

            if (role is null)
                return NotFound<GetRoleByIdResponse>(_localizer[SharedResourcesKeys.RoleNotExist]);

            var mappedRole = _mapper.Map<GetRoleByIdResponse>(role);

            return Success(mappedRole);
        }

        public async Task<Response<GetRolesForUserResponse>> Handle(GetRolesForUserQuery request, CancellationToken cancellationToken)
        {
            // Get All Roles But UserRoles Checked as HasRole = true..
            var roles = await _authorizationService.GetRolesForUserAsync(request.UserId);

            var result = new GetRolesForUserResponse
            {
                UserId = request.UserId,
                Roles = roles
            };

            return Success(result);
        }
    }
}
