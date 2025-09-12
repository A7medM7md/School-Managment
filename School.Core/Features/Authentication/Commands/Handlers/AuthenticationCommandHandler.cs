using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Resources;
using School.Data.Entities.Identity;
using School.Data.Helpers.JWT;
using School.Service.Abstracts;
using School.Service.Responses;

namespace School.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
                                                IRequestHandler<SignInCommand, Response<SignInResponse>>,
                                                IRequestHandler<RefreshTokenCommand, Response<SignInResponse>>,
                                                IRequestHandler<ValidateTokenCommand, Response<TokenValidationResponse>>
    {
        #region Fields

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion


        #region Constructors

        public AuthenticationCommandHandler(IMapper mapper,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        #endregion


        #region Handle Functions

        public async Task<Response<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // Check UserName and Password
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null) return BadRequest<SignInResponse>(_localizer[SharedResourcesKeys.InvalidNameOrPassword]);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded) return BadRequest<SignInResponse>(_localizer[SharedResourcesKeys.InvalidNameOrPassword]);

            // Generate Token [access & refresh Tokens]
            var response = await _tokenService.GenerateJwtTokenAsync(user);

            // Return Tokens
            return Success(response);
        }

        public async Task<Response<SignInResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
            return Success(result);
        }

        public async Task<Response<TokenValidationResponse>> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _tokenService.ValidateAccessToken(request.AccessToken);

            return Success(validationResult);
        }



        #endregion


    }
}
