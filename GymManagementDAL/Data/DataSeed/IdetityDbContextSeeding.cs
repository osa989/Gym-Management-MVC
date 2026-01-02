using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdetityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) // admin and super admin
        {
            try
            {
                var HasUsers = userManager.Users.Any();
                var HasRoles = roleManager.Roles.Any();

                if (HasUsers && HasRoles)
                    return false; // no need to seed

                if (!HasRoles) // roles first then users
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new (){Name = "Admin" },
                        new (){Name ="SuperAdmin"}
                    };
                    foreach (var Role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(Role.Name!).Result){
                            roleManager.CreateAsync(Role).Wait();
                        }
                    }
                }
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Osama",
                        LastName = "Ahmed",
                        UserName = "osama.admin",
                        Email = "osamagamal@gmail.com",
                        PhoneNumber = "01030772259"
                    };
                    userManager.CreateAsync(MainAdmin, "P@ssw0rd123").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();
                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Youssef",
                        LastName = "Ahmed",
                        UserName = "youssef.admin",
                        Email = "youssefgamal@gmail.com",
                        PhoneNumber = "01030872259"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd123").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true; // seeding done successfully
                // NO  need for save changes as userManager and roleManager handle that internally

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during seeding: {ex.Message}");
                return false;
            }
        }
    }
}
