using System.Web.Mvc;

namespace Sporty.Helper
{
    /// <summary>
    /// A JsonResult with ContentType of text/html and the serialized object contained within textarea tags
    /// </summary>
    /// <remarks>
    /// It is not possible to upload files using the browser's XMLHttpRequest
    /// object. So the jQuery Form Plugin uses a hidden iframe element. For a
    /// JSON response, a ContentType of application/json will cause bad browser
    /// behavior so the content-type must be text/html. Browsers can behave badly
    /// if you return JSON with ContentType of text/html. So you must surround
    /// the JSON in textarea tags. All this is handled nicely in the browser
    /// by the jQuery Form Plugin. But we need to overide the default behavior
    /// of the JsonResult class in order to achieve the desired result.
    /// </remarks>
    public class FileUploadJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            ContentType = "text/html";
            context.HttpContext.Response.Write("<textarea>");
            base.ExecuteResult(context);
            context.HttpContext.Response.Write("</textarea>");
        }
    }
}