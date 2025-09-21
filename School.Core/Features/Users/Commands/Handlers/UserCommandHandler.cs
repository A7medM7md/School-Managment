using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;
using School.Data.Commons;
using School.Data.Entities.Identity;
using School.Data.Helpers.Email;
using School.Service.Abstracts;
using System.Text;

namespace School.Core.Features.Users.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler,
                                        IRequestHandler<AddUserCommand, Response<string>>,
                                        IRequestHandler<EditUserCommand, Response<string>>,
                                        IRequestHandler<DeleteUserCommand, Response<string>>,
                                        IRequestHandler<ChangeUserPasswordCommand, Response<string>>
    {
        #region Fields

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion


        #region Constructors

        public UserCommandHandler(UserManager<AppUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole<int>> roleManager,
            IEmailService emailService,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _emailService = emailService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion


        #region Handle Functions

        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // Check If Email&UserName Already Exists
            bool isEmailExists = await _userManager.FindByEmailAsync(request.Email) is not null;
            if (isEmailExists) return BadRequest<string>(_localizer[SharedResourcesKeys.EmailIsExist]);

            bool isUserNameExists = await _userManager.FindByNameAsync(request.UserName) is not null;
            if (isUserNameExists) return BadRequest<string>(_localizer[SharedResourcesKeys.UserNameIsExist]);

            // Map Then Create New User
            var user = _mapper.Map<AppUser>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest<string>(IdentityErrorHelper.LocalizeErrors(result.Errors, _localizer));

            /// Determine Role [I Make Seeding For Admin, So It Is Not Useful Now]
            ///string roleName;
            ///if (await _userManager.Users.CountAsync() == 1)
            ///    roleName = "Admin";
            ///else
            ///    roleName = "User";

            // Ensure Role Exists [Double Check, I Already Add It By Seeding]
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole<int>("User"));

            // Add User To Admin Role
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToAddNewRoles]);


            // Generate Email Confirmation Token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var httpRequest = _httpContextAccessor.HttpContext?.Request;
            string baseUrl;

            if (httpRequest != null && httpRequest.Host.HasValue)
            {
                // 1) Get baseUrl from httpRequest
                baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";
            }
            else
            {
                // 2) In case U using a background job, Get baseUrl from config not request
                baseUrl = _config["FrontendBaseUrl"]!;
            }

            var confirmLink = $"{baseUrl}/api/v1/authentication/confirm-email?userId={user.Id}&token={tokenEncoded}";

            var emailContent = new EmailContent
            {
                Subject = "Confirm your email",
                RecipientName = user.FullName ?? user.UserName,
                LeadText = "Thanks for registering!",
                BodyText = "Please confirm your email by clicking the link below:",
                ActionLink = confirmLink,
                ActionText = "Confirm Email"
            };

            // Send Confirmation Email
            var sendResponse = await _emailService.SendEmailAsync(user.Email!, emailContent, cancellationToken);

            if (!sendResponse.Succeeded)
            {
                return Response<string>.Fail(
                    message: sendResponse.Message,
                    statusCode: sendResponse.StatusCode,
                    errors: sendResponse.Errors
                );
            }

            return Created($"User {request.FullName} created successfully. A confirmation email has been sent to {user.Email}.");
        }

        public async Task<Response<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user is null) return NotFound<string>();

            bool isUserNameExists = await _userManager.Users
                .FirstOrDefaultAsync(U => U.UserName == request.UserName && U.Id != request.Id, cancellationToken)
                is not null;

            if (isUserNameExists) return BadRequest<string>(_localizer[SharedResourcesKeys.UserNameIsExist]);

            _mapper.Map(request, user); // existing user updated by user in request

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Success<string>(_localizer[SharedResourcesKeys.Updated]);

            return BadRequest<string>(_localizer[SharedResourcesKeys.UpdateFailed]);
        }

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user is null) return NotFound<string>();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Deleted<string>(_localizer[SharedResourcesKeys.Deleted]);

            return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);
        }

        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user is null) return NotFound<string>();

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            /* OR [If User Created Without Password, If He Comes From OAuth]
               if(await _userManager.HasPasswordAsync(user)){
                   await _userManager.RemovePasswordAsync(user);
                   await _userManager.AddPasswordAsync(user, request.NewPassword);
               }
            */

            if (result.Succeeded)
                return Success("Password Changed Successfully");

            var errors = result.Errors
                    .Select(e =>
                    {
                        var localized = _localizer[e.Code];
                        return localized.ResourceNotFound ? e.Description : localized.Value;
                    }).ToList();

            if (!errors.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.FaildToAddUser]);

            return BadRequest<string>(string.Join(" | ", errors));
        }

        #endregion

    }
}
