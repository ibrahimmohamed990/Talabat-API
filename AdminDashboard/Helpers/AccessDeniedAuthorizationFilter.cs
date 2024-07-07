using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Helpers
{
    public class AccessDeniedAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Admin" }, { "action", "AccessDenied" } });
            }

            base.OnActionExecuting(context);
        }
    }

}
