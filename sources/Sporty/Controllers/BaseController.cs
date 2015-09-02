using System;
using System.Web.Mvc;
using System.Web.Routing;
using Sporty.Business.Interfaces;
using Sporty.Helper;
using Sporty.Infrastructure;

namespace Sporty.Controllers
{
    [HandleErrorWithElmah]
    public class BaseController : Controller
    {
        protected Guid? UserId;
        protected IUserRepository UserRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            UserRepository = ServiceFactory.Current.Resolve<IUserRepository>();
            UserId = GetUserId();
        }

        protected Guid? GetUserId()
        {
            if (!UserId.HasValue)
            {
                UserId = UserRepository.FindUserId(HttpContext.User.Identity.Name);
            }
            return UserId;
        }

        protected string GetCurrentUserId()
        {
            string name = HttpContext.User.Identity.Name;
            return name;
        }

        protected DateTime GetDateFromSessionOrToday()
        {
            DateTime currentDate = DateTime.Today;
            var dateInSession = Session["startDate"] as DateTime?;
            if (dateInSession.HasValue)
            {
                currentDate = dateInSession.Value;
            }
            return currentDate;
        }
    }
}