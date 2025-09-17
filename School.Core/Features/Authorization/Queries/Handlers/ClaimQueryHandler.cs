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
    public class ClaimQueryHandler : ResponseHandler,
                                        IRequestHandler<GetClaimsForUserQuery, Response<GetClaimsForUserResponse>>
    {
        private readonly IAuthorizationService _authorizationService;

        public ClaimQueryHandler(IAuthorizationService authorizationService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
        }


        public async Task<Response<GetClaimsForUserResponse>> Handle(GetClaimsForUserQuery request, CancellationToken cancellationToken)
        {
            var claimsResponse = await _authorizationService.GetClaimsForUserAsync(request.UserId);

            if (!claimsResponse.Succeeded)
            {
                return Response<GetClaimsForUserResponse>.Fail(
                    message: claimsResponse.Message,
                    statusCode: claimsResponse.StatusCode,
                    errors: claimsResponse.Errors
                );
            }

            var result = new GetClaimsForUserResponse
            {
                UserId = request.UserId,
                Claims = claimsResponse.Data
            };

            return Success(result);
        }
    }
}
