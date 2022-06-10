using IdentityApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages
{
    public class IndexModel : PageModel
    {
        public Dictionary<string, int> revenueSubmitted;
        public Dictionary<string, int> revenueApproved;
        public Dictionary<string, int> revenueRejected;

        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            initDict(ref revenueSubmitted); //ref is used because we use void else it would be NULL
            initDict(ref revenueApproved);  //if we used it with a return type
            initDict(ref revenueRejected);  //there would be no need in that

            var invoices = _context.Invoice.ToList(); //grab all invoices from DB and place to list

            foreach (var invoice in invoices)
            {
                switch (invoice.Status)
                {
                    case InvoiceStatus.Submitted:
                        revenueSubmitted[invoice.InvoiceMonth] += (int)invoice.InvoiceAmount;
                        break;
                    case InvoiceStatus.Approved:
                        revenueApproved[invoice.InvoiceMonth] += (int)invoice.InvoiceAmount;
                        break;
                    case InvoiceStatus.Rejected:
                        revenueRejected[invoice.InvoiceMonth] += (int)invoice.InvoiceAmount;
                        break;
                        
                }

                    //taking key as a month          //incrementing value
                //revenue[invoice.InvoiceMonth] += (int)invoice.InvoiceAmount;
                                               //this operator affect only a value
            }

        }
        private void initDict(ref Dictionary<string, int> dict)
        {
            dict = new Dictionary<string, int>()
            {
                {"January", 0},
                {"February", 0},
                {"March", 0},
                {"April", 0},
                {"May", 0},
                {"June", 0},
                {"July", 0},        // {[key], value}
                {"August", 0},      //named key - value pair
                {"September", 0},
                {"October", 0},
                {"November", 0},
                {"December", 0}
            };
        }
    }
}