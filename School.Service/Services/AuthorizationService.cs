using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Data.Commons;
using School.Data.Dtos;
using School.Data.Entities.Identity;
using School.Data.Helpers.AuthZ;
using School.Infrastructure.Bases;
using School.Service.Abstracts;
using School.Service.Responses;
using System.Data;
using System.Security.Claims;

namespace School.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepositoryAsync<IdentityRole<int>> _genericRepository;

        public AuthorizationService(RoleManager<IdentityRole<int>> roleManager,
                                        UserManager<AppUser> userManager,
                                        IGenericRepositoryAsync<IdentityRole<int>> genericRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _genericRepository = genericRepository;
        }


        #region Role Service Implementations

        public async Task<IdentityResult> AddRoleAsync(string roleName)
        {
            var role = new IdentityRole<int>(roleName);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<bool> IsRoleExist(string roleName, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _roleManager.Roles
                    .AsNoTracking()
                    .AnyAsync(r => r.Name == roleName && r.Id != excludeId.Value);
            }
            else
            {
                return await _roleManager.RoleExistsAsync(roleName);
            }
        }

        public async Task<IdentityResult> AssignRoleAsync(int userId, string roleName)
        {
            // Get user
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            // Assign role
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> EditRoleAsync(int id, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });

            role.Name = roleName;

            var result = await _roleManager.UpdateAsync(role);

            return result;
        }

        public async Task<RoleResult> DeleteRoleAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role is null)
                return new RoleResult { Status = RoleStatus.NotFound };

            var roleUsers = await _userManager.GetUsersInRoleAsync(role.Name!);

            if (roleUsers.Any())
                return new RoleResult { Status = RoleStatus.HasUsers, Role = role };

            var result = await _roleManager.DeleteAsync(role);

            return new RoleResult
            {
                Status = result.Succeeded ? RoleStatus.Success : RoleStatus.Failed,
                IdentityResult = result
            };
        }

        public async Task<IReadOnlyList<IdentityRole<int>>> GetRolesAsync(CancellationToken cancellationToken)
        {
            return await _roleManager.Roles
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);
        }

        public async Task<IdentityRole<int>?> GetRoleByIdAsync(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<IReadOnlyList<RoleDto>> GetRolesForUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Array.Empty<RoleDto>();

            var userRoles = await _userManager.GetRolesAsync(user);

            var allRoles = await _roleManager.Roles.AsNoTracking().ToListAsync();

            var result = allRoles.Select(role => new RoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name!,
                HasRole = userRoles.Contains(role.Name!)
            })
            .ToList();

            return result;
        }

        public async Task<IdentityResult> UpdateUserRolesAsync(int userId, IReadOnlyList<RoleDto> roles)
        {
            // Get User
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            // Get Current  Roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            #region Logic 1. Delete All Old Then Add New

            //// Remove Old Roles
            //var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            //if (!result.Succeeded)
            //    return IdentityResult.Failed(new IdentityError { Description = "Failed to remove old user roles." });

            //return await _userManager.AddToRolesAsync(user, roles.Where(r => r.HasRole == true).Select(r => r.RoleName));

            #endregion

            #region Logic 2. Diff Between Old And New, Then Add Only Changes

            // Get New Roles That Has HasRole = True
            var newRoles = roles.Where(r => r.HasRole).Select(r => r.RoleName).ToList();

            // Roles In Old But Not In New [To Remove]
            var rolesToRemove = currentRoles.Except(newRoles).ToList();

            // Roles In New But Not In Old [To Add]
            var rolesToAdd = newRoles.Except(currentRoles).ToList();

            if (!rolesToRemove.Any() && !rolesToAdd.Any())
                return IdentityResult.Success;

            using (var transaction = await _genericRepository.BeginTransactionAsync())
            {
                try
                {
                    // Remove Roles
                    if (rolesToRemove.Any())
                    {
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                        if (!removeResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return IdentityResult.Failed(removeResult.Errors.ToArray());
                        }
                    }

                    // Add Roles
                    if (rolesToAdd.Any())
                    {
                        var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                        if (!addResult.Succeeded)
                        {
                            // Rollback
                            await transaction.RollbackAsync();
                            return IdentityResult.Failed(addResult.Errors.ToArray());
                        }
                    }

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return IdentityResult.Failed(new IdentityError { Description = $"Unexpected error: {ex.Message}" });
                }
            }

            #endregion
        }

        #endregion


        #region Claim Service Implementation

        public async Task<Response<List<ClaimDto>>> GetClaimsForUserAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user is null)
                    return Response<List<ClaimDto>>.Fail(
                        message: $"User with ID {userId} not found",
                        statusCode: StatusCodes.Status404NotFound
                    );

                var userClaims = await _userManager.GetClaimsAsync(user);

                // Temporary From List Not DB
                var allClaims = ClaimsStore.claims;

                var result = allClaims.Select(claim => new ClaimDto
                {
                    ClaimType = claim.Type,
                    ClaimValue = userClaims.Any(uc => uc.Type == claim.Type)
                })
                .ToList();

                return Response<List<ClaimDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<List<ClaimDto>>.Fail(
                    message: "Failed to get user claims",
                    statusCode: StatusCodes.Status500InternalServerError,
                    errors: new List<string> { ex.Message + (ex.InnerException != null ? "\n" + ex.InnerException.Message : "") }
                );
            }
        }

        public async Task<IdentityResult> UpdateUserClaimsAsync(int userId, List<ClaimDto> claims)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            var currentClaims = await _userManager.GetClaimsAsync(user);

            var newClaims = claims
                .Where(r => r.ClaimValue)
                .Select(r => new Claim(r.ClaimType, "true"))
                .ToList();

            var claimsToRemove = currentClaims
                .Where(c => !newClaims.Any(nc => nc.Type == c.Type))
                .ToList();

            var claimsToAdd = newClaims
                .Where(nc => !currentClaims.Any(c => c.Type == nc.Type))
                .ToList();


            if (!claimsToRemove.Any() && !claimsToAdd.Any())
                return IdentityResult.Success;

            using (var transaction = await _genericRepository.BeginTransactionAsync())
            {
                try
                {
                    if (claimsToRemove.Any())
                    {
                        var removeResult = await _userManager.RemoveClaimsAsync(user, claimsToRemove);
                        if (!removeResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return IdentityResult.Failed(removeResult.Errors.ToArray());
                        }
                    }

                    if (claimsToAdd.Any())
                    {
                        var addResult = await _userManager.AddClaimsAsync(user, claimsToAdd);
                        if (!addResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return IdentityResult.Failed(addResult.Errors.ToArray());
                        }
                    }

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return IdentityResult.Failed(new IdentityError { Description = $"Unexpected error: {ex.Message}" });
                }
            }

        }



        #endregion

    }
}
