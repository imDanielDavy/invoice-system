using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IdentityApp.Authorization;

namespace IdentityApp.Pages.Invoices
{
    public class CreateModel : DI_BasePageModel
    {
        private readonly IdentityApp.Data.ApplicationDbContext _context;

        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager) { } //calling a ctor of a base class

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Invoice Invoice { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //telling that you are the creator of an Invoice
            Invoice.CreatorId = UserManager.GetUserId(User);

            //pass a User, Model and Create operation to a Handler
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, Invoice, InvoiceOperations.Create);
        
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            DbContext.Invoice.Add(Invoice);
            await DbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
