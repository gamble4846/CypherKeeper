using CypherKeeper.AuthLayer.Models;
using CypherKeeper.AuthLayer.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MongoDB.Driver;
using NuGet.Configuration;

namespace CypherKeeper.AuthLayer.ActionFilters
{
    public class LoginAuthorization : IAuthorizationFilter
    {
        public IConfiguration Configuration;
        IHttpContextAccessor HttpContextAccessor;

        public LoginAuthorization(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var _CF = new CommonFunctions(Configuration, HttpContextAccessor);
                var IsTokenValid = _CF.ValidateCurrentToken();
                if (!IsTokenValid)
                {
                    context.Result = new ForbidResult("Invalid Token");
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Result = new ForbidResult(ex.Message + " Invalid Token");
                return;
            }
        }
    }

    public class LoginAuthorizationAttribute : TypeFilterAttribute
    {
        public LoginAuthorizationAttribute() : base(typeof(LoginAuthorization))
        {
            Arguments = new object[] { };
        }
    }
}
