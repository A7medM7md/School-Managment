using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;
using School.Data.Entities.Identity;

namespace School.Core.Features.Users.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler,
                                        IRequestHandler<AddUserCommand, Response<string>>,
                                        IRequestHandler<EditUserCommand, Response<string>>
    {
        #region Fields

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion


        #region Constructors

        public UserCommandHandler(UserManager<AppUser> userManager,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _localizer = localizer;
            _userManager = userManager;
            _mapper = mapper;
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

            if (result.Succeeded)
                // Return Success Creation
                return Created($"User: {request.FullName} Is Created Successfully");

            // Handle Identity Errors with Localization [Localization Not Added For Now]
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

        public async Task<Response<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByIdAsync(request.Id.ToString());

            if (existingUser is null) return NotFound<string>();

            var newUser = _mapper.Map(request, existingUser);

            var result = await _userManager.UpdateAsync(newUser);

            if (result.Succeeded)
                return Success<string>(_localizer[SharedResourcesKeys.Updated]);

            return BadRequest<string>(_localizer[SharedResourcesKeys.UpdateFailed]);
        }

        #endregion

    }
}
