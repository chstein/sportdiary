﻿using System;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace Sporty.Helper
{
    public class HandleErrorWithElmahAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);


            Exception e = context.Exception;

            if (!context.ExceptionHandled // if unhandled, will be logged anyhow
                || RaiseErrorSignal(e) // prefer signaling, if possible
                || IsFiltered(context)) // filtered?

                return;


            LogException(e);
        }


        private static bool RaiseErrorSignal(Exception e)
        {
            HttpContext context = HttpContext.Current;

            if (context == null)

                return false;

            ErrorSignal signal = ErrorSignal.FromContext(context);

            if (signal == null)

                return false;

            signal.Raise(e, context);

            return true;
        }


        private static bool IsFiltered(ExceptionContext context)
        {
            var config = context.HttpContext.GetSection("elmah/errorFilter") as ErrorFilterConfiguration;

            if (config == null)

                return false;


            var testContext = new ErrorFilterModule.AssertionHelperContext(context.Exception, HttpContext.Current);

            return config.Assertion.Test(testContext);
        }


        private static void LogException(Exception e)
        {
            HttpContext context = HttpContext.Current;

            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }
    }
}