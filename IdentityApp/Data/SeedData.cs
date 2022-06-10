using IdentityApp.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Data
{
    public class SeedData
    {

        public static async Task Initialize(
            IServiceProvider serviceProvider, string password)
        {
            //we cant use DI so we create our context for a moment of this particular method
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // manager role
                var managerUid = await EnsureUser(serviceProvider, "manager@demo.com", password);
                await EnsureRole(serviceProvider, managerUid, Constants.InvoiceManagersRole);

                // admin role
                var adminUid = await EnsureUser(serviceProvider, "admin@demo.com", password);
                await EnsureRole(serviceProvider, adminUid, Constants.InvoiceAdminsRole);
            }
        }

        //code for creating a new user / called only from initialize method
        private static async Task<string> EnsureUser(IServiceProvider serviceProvier, //to use UserManager
            string userName, string initPw)
        {                                                 //api to manage a user
            var userManager = serviceProvier.GetService<UserManager<IdentityUser>>();
            
            //passes true or false if a user already existing 
            var user = await userManager.FindByNameAsync(userName);

            //if not then create one
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };
                
                //register in db
                var result = await userManager.CreateAsync(user, initPw);
            }

            //check if successfuly created or there are any errors
            if (user == null)
            {
                throw new Exception("User did not get created. Password policy problem?");
            }

            return user.Id;
        }

        //assign a role to a specific user / called only from initialize method
        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider, //to be able to use RoleManager
            string uid, string role)
        {
            //Api to manage roles
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult identityResult;

            //if this specific role does not exist - create it
            if(await roleManager.RoleExistsAsync(role) == false)
            {
                identityResult = await roleManager.CreateAsync(new IdentityRole(role));
            }

            //API to manage a user
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //grabbing a user
            var user = await userManager.FindByIdAsync(uid);

            if(user == null)
            {
                throw new Exception("User not existing");
            }

            //add a user to a role
            identityResult = await userManager.AddToRoleAsync(user, role);

            return identityResult; // return the result of Identity Operation

        }
    }

}
