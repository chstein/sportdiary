using System;
using System.Globalization;
using System.Web.Mvc;

namespace Sporty.Helper
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (result == null)
                return null;

            string value = result.AttemptedValue;
            if (String.IsNullOrEmpty(value))
                return null;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName,
                                                    bindingContext.ValueProvider.GetValue(bindingContext.ModelName));

            DateTime dt;
            bool success = DateTime.TryParse(value, CultureInfo.GetCultureInfo("de-DE"), DateTimeStyles.None, out dt);

            if (!success)
            {
                //try another format like //value can be 22_03_2013
                success = DateTime.TryParseExact(value, "dd_MM_yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            }
            return success ? dt : new DateTime();
        }
    }
}