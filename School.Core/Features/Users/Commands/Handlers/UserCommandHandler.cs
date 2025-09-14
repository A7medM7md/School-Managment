using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;
using School.Data.Entities.Identity;

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

        #endregion


        #region Constructors

        public UserCommandHandler(UserManager<AppUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole<int>> roleManager,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
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

            // Determine Role
            string roleName;
            if (await _userManager.Users.CountAsync() == 1)
                roleName = "Admin";
            else
                roleName = "User";

            // Ensure Role Exists
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole<int>(roleName));

            // Add User To Admin Role
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToAddNewRoles]);

            return Created($"User: {request.FullName} created successfully with role {roleName}");
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
