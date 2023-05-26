using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using AuthLayer.Mangers.Interface;

namespace AuthLayer.ActionFilters
{
    public class RoleAuthorization : IAuthorizationFilter
    {
        private string _Contoller;
        private string _Action;
        public IConfiguration Configuration;
        public string ContentRootPath;
        IHttpContextAccessor HttpContextAccessor;
        public IUserManager UserManager;
        public IRoleManager RoleManager;

        public RoleAuthorization(IRoleManager roleManager, string _CustomController, string _CustomAction, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment env, IUserManager manager)
        {
            _Contoller = _CustomController;
            _Action = _CustomAction;
            Configuration = configuration;
            ContentRootPath = env.ContentRootPath;
            HttpContextAccessor = httpContextAccessor;
            UserManager = manager;
            RoleManager = roleManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (String.IsNullOrEmpty(_Contoller))
            {
                _Contoller = (string)context.RouteData.Values["controller"];
            }

            if (String.IsNullOrEmpty(_Action))
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                _Action = descriptor.ActionName;
            }

            var isAllowed = (bool)RoleManager.GetIfUserHasAccessToControllerAction(_Contoller, _Action).Document;

            if (!isAllowed)
            {
                context.Result = new ForbidResult("You Dont Have Access To Controller - " + _Contoller + " And Action - " + _Action);
                return;
            }
        }
    }

    public class RoleAuthorizationAttribute : TypeFilterAttribute
    {
        public RoleAuthorizationAttribute(string _CustomController, string _CustomAction) : base(typeof(RoleAuthorization))
        {
            Arguments = new object[] { _CustomController, _CustomAction };
        }

        public RoleAuthorizationAttribute() : base(typeof(RoleAuthorization))
        {
            Arguments = new object[] { "", "" };
        }
    }
}
