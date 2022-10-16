using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pantree.Data.Models;
using Pantree.Helpers;
using Pantree.Helpers.Extentions;
using Pantree.Services;
using Pantree.Store.Models;

namespace Pantree.Store.Controllers
{
    public class BaseController<T> : Controller
    {
        private ILogger<T> _logger;
        protected ILogger<T> Logger => _logger ?? HttpContext.RequestServices.GetService<ILogger<T>>();

        public User User { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            User = UserMethods.PantreeUser(null, HttpContext, AppConfig.CookieKey);

            if (User.IsAuthenticated)
                User.UserDetails = new UserService(AppConfig.ConnectionString).GetFullUserData(User.Username);

            ViewBag.User = User;
        }
    }

    public class AuthorizedUser : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User user = UserMethods.PantreeUser(null, filterContext.HttpContext, AppConfig.CookieKey);

            var controller = filterContext.RouteData.Values["controller"];
            var action = filterContext.RouteData.Values["action"];

            if (!user.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult("Default",
                    new RouteValueDictionary()
                    {
                        { "controller", "Account" },
                        { "action", "Index" },
                        { "redirect", $"{controller}/{action}" }
                    });
            }
        }
    }
}