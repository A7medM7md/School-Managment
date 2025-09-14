using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Authorization.Commands.Models;
using School.Core.Resources;
using School.Service.Abstracts;

namespace School.Core.Features.Authorization.Commands.Handlers
{
    public class RoleCommandHandler : ResponseHandler,
                                        IRequestHandler<AddRoleCommand, Response<string>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public RoleCommandHandler(IAuthorizationService authorizationService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.AddRoleAsync(request.RoleName);
            if (!result.Succeeded)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AddFailed]);

            return Created(request.RoleName, _stringLocalizer[SharedResourcesKeys.RoleAddedSuccessfully]);
        }
    }
}
