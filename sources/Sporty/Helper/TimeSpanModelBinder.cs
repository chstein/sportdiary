using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Sporty.Helper
{
    public class TimeSpanModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <example>
        /// 30:00 -> 00:30:00
        /// 30 -> 00:30:00
        /// </example>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>
        /// The bound object.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="bindingContext "/>parameter is null.</exception>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;
            if (String.IsNullOrEmpty(value))
                return null;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName,
                                                    bindingContext.ValueProvider.GetValue(bindingContext.ModelName));
            try
            {
                //check 1:30:00
                {
                    var regex = new Regex(@"^([0-9]?[0-9]):([0-5][0-9]):([0-5][0-9])$");
                    Match match = regex.Match(value);
                    if (match.Success)
                    {
                        var parsedSpan = new TimeSpan(Int32.Parse(match.Groups[1].Value),
                                                      Int32.Parse(match.Groups[2].Value),
                                                      Int32.Parse(match.Groups[3].Value));
                        return parsedSpan;
                    }
                }
                //check 30:00
                {
                    var regex = new Regex(@"^([0-5]?[0-9]):([0-5][0-9])$");
                    Match match = regex.Match(value);
                    if (match.Success)
                    {
                        var parsedSpan = new TimeSpan(0, Int32.Parse(match.Groups[1].Value),
                                                      Int32.Parse(match.Groups[2].Value));
                        return parsedSpan;
                    }
                }
                //check 30 
                {
                    var regex = new Regex(@"^([0-9]+)$");
                    Match match = regex.Match(value);
                    if (match.Success)
                    {
                        var parsedSpan = new TimeSpan(0, Int32.Parse(match.Groups[1].Value), 0);
                        return parsedSpan;
                    }
                }
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                                                        String.Format("\"{0}\" is invalid.", bindingContext.ModelName));
                return null;
            }
            catch (Exception)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                                                        String.Format("\"{0}\" is invalid.", bindingContext.ModelName));
                return null;
            }
        }
    }
}