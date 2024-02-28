using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Operators;

namespace WebStore.API.LoginData
{
    public static class SeedAdministratorRoleAndUser
    {
        internal async static Task Seed(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> customerRoleManager)
        {
            await SeedAdministratorRole(roleManager);
            await SeedAdministratorUser(userManager);
            await SeedCustomerRole(customerRoleManager);
        }

        private async static Task SeedAdministratorRole(RoleManager<IdentityRole> roleManager)
        {

            bool administratorRoleExists = await roleManager.RoleExistsAsync("Administrator");

            if (administratorRoleExists == false)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                await roleManager.CreateAsync(role);
            }
        }

        private async static Task SeedCustomerRole(RoleManager<IdentityRole> customerRoleManager)
        {
            bool customerRoleExists = await customerRoleManager.RoleExistsAsync("Customer");

            if (customerRoleExists == false)
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                await customerRoleManager.CreateAsync(role);
            }
        }

        private async static Task SeedAdministratorUser(UserManager<IdentityUser> userManager)
        {
            bool administratorUserExists = await userManager.FindByEmailAsync("admin@webstore.com") != null;

            if (administratorUserExists == false)
            {
                var administratorUser = new IdentityUser
                {
                    UserName = "admin@webstore.com",
                    Email = "admin@webstore.com"
                };
                IdentityResult identityResult = await userManager.CreateAsync(administratorUser, "Password1!");

                if (identityResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(administratorUser, "Administrator");
                }
            }
        }
    }
}
