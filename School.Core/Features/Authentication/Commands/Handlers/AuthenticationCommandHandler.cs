using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Resources;
using School.Data.Entities.Identity;
using School.Service.Abstracts;

namespace School.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
                                                IRequestHandler<SignInCommand, Response<string>>
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

        public async Task<Response<string>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // Check UserName and Password
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null) return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidNameOrPassword]);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded) return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidNameOrPassword]);

            // Generate Token
            var accessToken = await _tokenService.GenerateJwtTokenAsync(user);

            // Return Token
            return Success(accessToken);
        }



        #endregion


    }
}
