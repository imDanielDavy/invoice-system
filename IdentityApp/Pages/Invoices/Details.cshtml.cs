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
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager) { }

        public Invoice Invoice { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || DbContext.Invoice == null)
            {
                return NotFound();
            }

            var invoice = await DbContext.Invoice.FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }
            else 
            {
                Invoice = invoice;
            }

            var isCreator = await AuthorizationService.AuthorizeAsync(
                User, Invoice, InvoiceOperations.Read);

            //if true - we have a manager
            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            if (isCreator.Succeeded == false && isManager == false)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, InvoiceStatus status)
        {
            Invoice = await DbContext.Invoice.FindAsync(id); //we refer to a db table and look for needed id

            if(Invoice == null)
            {
                return NotFound();
            }

            //if a user clicks a button and
            //status = Approved: true - Approve op, false - Reject op
            var invoiceOperation = status == InvoiceStatus.Approved
                ? InvoiceOperations.Approve
                : InvoiceOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, Invoice, invoiceOperation); //call a matching handler which allowed to use invoiceOperation

            if (isAuthorized.Succeeded == false)
            {
                return Forbid();
            }

            Invoice.Status = status; // change status in db
            DbContext.Invoice.Update(Invoice); //update db, thats why we use DbContext

            await DbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
