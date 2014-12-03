//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Sporty.Controllers
//{
//    public class RedirectController : Controller
//    {
//        //
//        // GET: /Redirect/

//        public RedirectToRouteResult NextMonth()
//        {
//            int monthValue = 0, yearValue = 0;
//            if (ViewData.ContainsKey("CurrentMonth"))
//                monthValue = Int32.Parse(ViewData["CurrentMonth"].ToString());
//            if (ViewData.ContainsKey("CurrentMonth"))
//                yearValue = Int32.Parse(ViewData["CurrentYear"].ToString());

//            if (monthValue == 12)
//            {
//                monthValue = 1;
//                yearValue++;
//            }

//            return RedirectToAction("Calendar", "Home", new { month = monthValue, year = yearValue });
//        }

//        public RedirectToRouteResult PreviousMonth()
//        {
//            int monthValue = 0, yearValue = 0;
//            if (ViewData.ContainsKey("CurrentMonth"))
//                monthValue = Int32.Parse(ViewData["CurrentMonth"].ToString());
//            if (ViewData.ContainsKey("CurrentMonth"))
//                yearValue = Int32.Parse(ViewData["CurrentYear"].ToString());

//            if (monthValue == 1)
//            {
//                monthValue = 12;
//                yearValue--;
//            }

//            return RedirectToAction("Calendar", "Home", new { month = monthValue, year = yearValue });
//        }
//    }
//}

