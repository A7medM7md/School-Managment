using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Authentication.Queries.Models;
using School.Core.Resources;
using School.Data.Commons;
using School.Service.Abstracts;

namespace School.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationQueryHandler : ResponseHandler,
                                                    IRequestHandler<VerifyResetPasswordCodeQuery, Response<bool>>
    {
        #region Fields

        private readonly IPasswordResetService _passwordResetService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion


        #region Constructors

        public AuthenticationQueryHandler(IPasswordResetService passwordResetService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _passwordResetService = passwordResetService;
        }

        #endregion


        #region Handle Functions

        public async Task<Response<bool>> Handle(VerifyResetPasswordCodeQuery request, CancellationToken cancellationToken)
        {
            var result = await _passwordResetService.VerifyResetCodeAsync(request.Email, request.ResetCode);

            return result.StatusCode switch
            {
                200 => Success(result.Data, result.Message),
                400 => BadRequest<bool>(result.Message),
                404 => NotFound<bool>(_localizer[SharedResourcesKeys.UserIsNotFound]),
                _ => InternalServerError<bool>(result.Message)
            };
        }


        #endregion


    }
}
