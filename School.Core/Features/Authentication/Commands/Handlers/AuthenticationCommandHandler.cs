using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Resources;
using School.Data.Commons;
using School.Data.Entities.Identity;
using School.Data.Helpers.Email;
using School.Data.Helpers.JWT;
using School.Service.Abstracts;
using School.Service.Responses;
using System.Text;

namespace School.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
                                                IRequestHandler<SignInCommand, Response<SignInResponse>>,
                                                IRequestHandler<RefreshTokenCommand, Response<SignInResponse>>,
                                                IRequestHandler<ValidateTokenCommand, Response<TokenValidationResponse>>,
                                                IRequestHandler<ConfirmEmailCommand, Response<string>>,
                                                IRequestHandler<SendResetPasswordCodeCommand, Response<string>>
    {
        #region Fields

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IPasswordResetService _passwordResetService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion


        #region Constructors

        public AuthenticationCommandHandler(IMapper mapper,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IEmailService emailService,
            IPasswordResetService passwordResetService,
            IConfiguration config,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _passwordResetService = passwordResetService;
            _config = config;
            _mapper = mapper;
        }

        #endregion


        #region Handle Functions

        public async Task<Response<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // Check UserName and Password
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
                return BadRequest<SignInResponse>(_localizer[SharedResourcesKeys.InvalidNameOrPassword]);

            // CheckPasswordSignInAsync: Check On 3 Things => Password + Lockout + ConfirmedEmail
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return Response<SignInResponse>.Fail(_localizer[SharedResourcesKeys.AccountLocked], 423); // 423 Locked

                if (result.IsNotAllowed)
                {
                    if (!user.EmailConfirmed)
                    {
                        // Generate new token
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                        // Generate confirm link
                        var baseUrl = _config["FrontendBaseUrl"]!;
                        var confirmLink = $"{baseUrl}/api/v1/authentication/confirm-email?userId={user.Id}&token={tokenEncoded}";

                        // Create email content
                        var emailContent = new EmailContent
                        {
                            Subject = "Confirm your email",
                            RecipientName = user.FullName ?? user.UserName,
                            LeadText = "Almost there!",
                            BodyText = "Please confirm your email by clicking the link below:",
                            ActionLink = confirmLink,
                            ActionText = "Confirm Email"
                        };

                        // Send email
                        await _emailService.SendEmailAsync(user.Email!, emailContent, cancellationToken);

                        return Response<SignInResponse>.Fail(
                            _localizer[SharedResourcesKeys.EmailNotConfirmed] + " A new confirmation email has been sent.",
                            403
                        );
                    }

                    return Response<SignInResponse>.Fail(_localizer[SharedResourcesKeys.EmailNotConfirmed], 403);
                }

                return BadRequest<SignInResponse>(_localizer[SharedResourcesKeys.InvalidNameOrPassword]);
            }

            // Generate Token [access & refresh Tokens]
            var response = await _tokenService.GenerateJwtTokenAsync(user);

            // Return Tokens
            return Success(response);
        }

        public async Task<Response<SignInResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

            if (!result.Success)
            {
                return result.ErrorCode switch
                {
                    400 => BadRequest<SignInResponse>(_localizer[result.ErrorMessage]),
                    401 => Unauthorized<SignInResponse>(_localizer[result.ErrorMessage]),
                    404 => NotFound<SignInResponse>(_localizer[result.ErrorMessage]),
                    _ => BadRequest<SignInResponse>()
                };
            }

            return Success(result.Data!); // ! => Null-forgiving operator, ya compiler this value never be null!
        }

        public async Task<Response<TokenValidationResponse>> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _tokenService.ValidateAccessToken(request.AccessToken);

            if (!result.Success)
            {
                return result.ErrorCode switch
                {
                    400 => BadRequest<TokenValidationResponse>(_localizer[result.ErrorMessage]),
                    401 => Unauthorized<TokenValidationResponse>(_localizer[result.ErrorMessage]),
                    404 => NotFound<TokenValidationResponse>(_localizer[result.ErrorMessage]),
                    _ => BadRequest<TokenValidationResponse>()
                };
            }

            return Success(result.Data!);
        }

        public async Task<Response<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if user exists
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.UserIsNotFound]);

            if (user.EmailConfirmed)
                return BadRequest<string>(_localizer[SharedResourcesKeys.EmailAlreadyConfirmed]);

            // 2. Decode the token
            string decodedToken;
            try
            {
                var bytes = WebEncoders.Base64UrlDecode(request.Token);
                decodedToken = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidTokenFormat]);
            }

            // 3. Confirm email
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                return BadRequest<string>(_localizer[SharedResourcesKeys.ErrorWhenConfirmEmail]);

            // 4. Success
            return Success<string>(_localizer[SharedResourcesKeys.ConfirmEmailDone]);
        }

        public async Task<Response<string>> Handle(SendResetPasswordCodeCommand request, CancellationToken cancellationToken)
        {
            var result = await _passwordResetService.GenerateAndSendResetCodeAsync(request.Email, cancellationToken);

            if (!result.Succeeded)
                return result.StatusCode switch
                {
                    404 => NotFound<string>(_localizer[SharedResourcesKeys.UserIsNotFound]),
                    500 => InternalServerError<string>(_localizer[SharedResourcesKeys.TryRequestCodeAgain], result.Errors),
                    _ => BadRequest<string>(_localizer[result.Message])
                };

            return Success(result.Data);
        }


        #endregion


    }
}
