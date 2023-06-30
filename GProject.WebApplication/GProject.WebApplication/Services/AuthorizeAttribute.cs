using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using GProject.WebApplication.Helpers;
using GProject.Data.DomainClass;

namespace GProject.WebApplication.Services
{
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var customer = context.HttpContext.Session.GetObjectFromJson<Customer>("userLogin");
            if (customer == null)
            {
                var returnUrl = context.HttpContext.Request.Path;
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl }));
            }
        }
    }
}
