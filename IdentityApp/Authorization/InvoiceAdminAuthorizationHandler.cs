using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IdentityApp.Authorization
{
    //every Authorization handler is called when somewhere in code AuthorizeAsync() is called
    //and depending on what Operation a user wants to use it choose between all existing handlers
    //the exact one which will meet all the requirements and use it particurarly
    public class InvoiceAdminAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Invoice invoice)
        {
            
            if(context.User == null || invoice == null)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(Constants.InvoiceAdminsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
