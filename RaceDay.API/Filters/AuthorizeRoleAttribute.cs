using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.API.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public AuthorizeRoleAttribute(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. Check if user is logged in (Session has UserID)
            var userId = context.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Please log in to access this resource." });
                return;
            }

            // 2. Check if user has the correct role
            var userRole = context.HttpContext.Session.GetString("UserRole");
            if (!string.IsNullOrEmpty(_requiredRole) && userRole != _requiredRole)
            {
                context.Result = new ObjectResult(new { message = "Forbidden: You do not have permission to perform this action." })
                {
                    StatusCode = 403
                };
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}