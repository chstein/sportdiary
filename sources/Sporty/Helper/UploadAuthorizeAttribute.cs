using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sporty.Helper
{
    public class UploadAuthorizeAttribute : AuthorizeAttribute
    {
        private const string TokenKey = "token";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string token = httpContext.Request.Params[TokenKey];

            if (token != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null)
                {
                    var identity = new FormsIdentity(ticket);

                    // Your roles and/or validation logic goes here.
                    var roles = new string[0];

                    var principal = new GenericPrincipal(identity, roles);
                    httpContext.User = principal;
                }
            }

            return base.AuthorizeCore(httpContext);
        }
    }
}