using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Authorization;

namespace IdentityApp.Pages.Invoices
{
    [AllowAnonymous] // makes index page entirely accessible for an unauthenticated users (like in sm apps)
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager) { }

        public IList<Invoice> Invoice { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //All Invoices
            var invoices = from i in DbContext.Invoice
                           select i;

            //Roles
            var isManager = User.IsInRole(Constants.InvoiceManagersRole);
            var isAdmin = User.IsInRole(Constants.InvoiceAdminsRole);

            //User
            var currentUserId = UserManager.GetUserId(User);

            //if user is NOT a manager show only invoices created by CurrentUser 
            if (!isManager && !isAdmin)
            {
                invoices = invoices.Where(i => i.CreatorId == currentUserId);
            }

            //else show all
            Invoice = await invoices.ToListAsync();
        }
    }
}
