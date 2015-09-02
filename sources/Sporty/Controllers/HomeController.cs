using System.Web.Mvc;

namespace Sporty.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Calendar", "Exercise");
            }
            return View();
        }



        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Imprint()
        {
            return View();
        }
    }
}