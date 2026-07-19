using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Persistence.SeedData
{
    public static class SeedData
    {
        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            foreach (var roleName in UserRoles.All)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                    continue;

                var result = await roleManager.CreateAsync(new AppRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        public static async Task SeedSuperAdminUserAsync(UserManager<AppUser> userManager)
        {
            const string email = "admin@taskmanagement.az";
            const string password = "Admin@123456";

            var superAdmin = await userManager.FindByEmailAsync(email);

            if (superAdmin is null)
            {
                superAdmin = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = email,
                    Email = email,
                    FullName = "Super Admin",
                    EmailConfirmed = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    CompanyName = "TaskManagement"
                };

                var createResult = await userManager.CreateAsync(superAdmin, password);

                if (!createResult.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                superAdmin.UserName = email;
                superAdmin.Email = email;
                superAdmin.EmailConfirmed = true;
                superAdmin.IsDeleted = false;

                if (string.IsNullOrWhiteSpace(superAdmin.FullName))
                    superAdmin.FullName = "Super Admin";

                var updateResult = await userManager.UpdateAsync(superAdmin);

                if (!updateResult.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                }
            }


            var passwordValid = false;
            try
            {
                passwordValid = await userManager.CheckPasswordAsync(superAdmin, password);
            }
            catch
            {
                passwordValid = false;
            }

            if (!passwordValid)
            {
                if (!string.IsNullOrWhiteSpace(superAdmin.PasswordHash))
                {
                    var removePasswordResult = await userManager.RemovePasswordAsync(superAdmin);

                    if (!removePasswordResult.Succeeded)
                    {
                        throw new Exception(
                            string.Join(", ", removePasswordResult.Errors.Select(e => e.Description)));
                    }
                }

                var addPasswordResult = await userManager.AddPasswordAsync(superAdmin, password);

                if (!addPasswordResult.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", addPasswordResult.Errors.Select(e => e.Description)));
                }
            }


            if (!await userManager.IsInRoleAsync(superAdmin, UserRoles.SuperAdmin))
            {
                var roleResult = await userManager.AddToRoleAsync(superAdmin, UserRoles.SuperAdmin);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            await SeedSuperAdminUserAsync(userManager);
        }
    }
}
