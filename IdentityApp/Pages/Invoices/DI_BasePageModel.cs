using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Data;

namespace IdentityApp.Pages.Invoices
{
    public class DI_BasePageModel : PageModel
    {
        //protected is used because we will use inheritance
        //and each child element inherited from this class will be able to access it
        protected ApplicationDbContext DbContext { get; } //allow to use db and its tables
        protected IAuthorizationService AuthorizationService { get; } //Checks policy based permissions for a user
        protected UserManager<IdentityUser> UserManager { get; } // allows to manage a user

        public DI_BasePageModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
        {
            DbContext = context;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }
    }
}
