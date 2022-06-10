using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Models;

namespace IdentityApp.Authorization
{
    //every Authorization handler is called when somewhere in code AuthorizeAsync() is called
    //and depending on what Operation a user wants to use it choose between all existing handlers
    //the exact one which will meet all the requirements and use it particurarly
    public class InvoiceCreatorAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        UserManager<IdentityUser> _userManager; //local variable
        
        //data passed with DI is used in this ctor to pass it to local variable
        public InvoiceCreatorAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            //currently loggedIn user
            _userManager = userManager;
        }
        

        //a checker for a user if he can use any operations
        //all authorization requirements are implemented here
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, Invoice invoice)
        {
            //if no user loggedIn or there is no invoice - exit
            if(context.User == null || invoice == null)
                return Task.CompletedTask; //something gone wrong

            //check if it is CRUD Operations a user wants to use.
            //If true - exit
            if(requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName && 
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask; //something gone wrong
            }

            //if I'm the creator of an Invoice only then I'm allowed to perform operations
            if (invoice.CreatorId == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement); //called only when all requirements are met
            }

            return Task.CompletedTask; // if every task failed - exit
        }
    }
}
