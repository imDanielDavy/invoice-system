using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IdentityApp.Authorization
{
    //it register a operation which has a type of a requirement so in future we can
    //use it to satisfy needs of handlers and therefore switch beetween roles
    public class InvoiceOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };

        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };

        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };

        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };

        public static OperationAuthorizationRequirement Approve =
            new OperationAuthorizationRequirement { Name = Constants.ApprovedOperationName };

        public static OperationAuthorizationRequirement Reject =
            new OperationAuthorizationRequirement { Name = Constants.RejectedOperationName };

    }

    //Constants are used to avoide strings inside a code
    public class Constants
    {
        //CRUD operations
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";

        //Our local operations
        public static readonly string ApprovedOperationName = "Approved";
        public static readonly string RejectedOperationName = "Rejected";

        //Roles
        public static readonly string InvoiceManagersRole = "InvoiceManager";
        public static readonly string InvoiceAdminsRole = "InvoiceAdmin";

    }
}
