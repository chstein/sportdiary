using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sporty.Business.Interfaces;
using System.Web.Routing;
using Sporty.Infrastructure;
using MvcContrib.UI.Grid;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using Sporty.ViewModel;
using System.Text;
using System.Net;
using System.IO;
using Common.Logging;
using Sporty.Helper;
using FineUploader;
using Sporty.Business.Helper;

namespace Sporty.Controllers
{
    [Authorize]
    public class MaterialController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MaterialController));

        private IMaterialRepository materialRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (UserId.HasValue)
            {
                materialRepository = ServiceFactory.Current.Resolve<IMaterialRepository>();
            }
        }

        //
        // GET: /Material/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int? page, GridSortOptions sort, string search)
        {
            IEnumerable<MaterialView> filteredMaterials = null;

            if (!String.IsNullOrEmpty(search))
            {
                filteredMaterials =
                    materialRepository.GetMaterials(UserId).Where(
                        g => g.Description != null && (g.Description.Contains(search))
                             || (g.Name != null && g.Name.Contains(search)));
            }
            else
            {
                if (filteredMaterials == null)
                {
                    filteredMaterials = materialRepository.GetMaterials(UserId);
                }
            }
            filteredMaterials = sort.Column != null
                                ? filteredMaterials.OrderBy(sort.Column, sort.Direction)
                                : filteredMaterials.OrderBy("InUsage", SortDirection.Descending);

            if (sort.Column != null)
            {
                filteredMaterials = filteredMaterials.OrderBy(sort.Column, sort.Direction);
            }

            ViewData["sort"] = sort;
            IPagination<MaterialView> pagedMetrics = filteredMaterials.AsPagination(page ?? 1, 25);
            return View(pagedMetrics);
        }

        //
        // GET: /Material/Create

        public ActionResult Create()
        {
            var material = new MaterialView();
            material.IsNew = true;

            if (Request != null && Request.IsAjaxRequest())
                return PartialView("_Edit", material);
            return View("_Edit", material);
        }

        //
        // GET: /Material/Edit/5

        public ActionResult Edit(int id)
        {
            MaterialView material = materialRepository.GetElement(UserId, id);

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("_Edit", material);
            }

            return View("_Edit", material);
        }

        //
        // POST: /Material/Edit/5

        [HttpPost]
        [OutputCache(CacheProfile = "ZeroCacheProfile")]
        public ActionResult Edit(MaterialView materialView)
        {
            int statusCode = 0;
            StringBuilder errorMessage = null;
            string resultMsg = string.Empty;


            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", materialView);
                }
                return PartialView("_Edit", materialView);
            }

            try
            {
                materialRepository.Save(UserId.Value, materialView);
                resultMsg = "<span style='color: blue'>Der Eintrag wurde erfolgreich gespeichert.</span>";
            }
            catch (Exception exp)
            {
                errorMessage = new StringBuilder(200);
                errorMessage.AppendFormat("<div class=\"validation-summary-errors\" title=\"Server Error\">{0}</div>",
                                          exp.GetBaseException().Message);
                statusCode = (int)HttpStatusCode.InternalServerError;

            }
            if (Request.IsAjaxRequest())
            {
                if (statusCode > 0)
                {
                    Response.StatusCode = statusCode;
                    return Content(errorMessage.ToString());
                }
                TempData["message"] = resultMsg;
                return Content(resultMsg);
            }
            return RedirectToAction("Index");
        }

        public FineUploaderResult ImageUpload(FineUpload upload, int id)
        {
            if (materialRepository == null)
            {
                materialRepository = ServiceFactory.Current.Resolve<IMaterialRepository>();
            }
            var filePathAndName = string.Empty;
            try
            {
                var filename = string.Format("{0}_{1}", id, upload.Filename);
                string extension = Path.GetExtension(filename).ToLower();
                filePathAndName = FileHelper.GetMaterialFilePathAndName(filename, UserId.Value.ToString());
                string directory = Path.GetDirectoryName(filePathAndName);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filename).ToLower();

                for (int i = 1; ; ++i)
                {
                    if (!System.IO.File.Exists(filePathAndName))
                        break;

                    filePathAndName = Path.Combine(directory, fileNameWithoutExt + "_" + i + extension);
                }

                var imagefilter = new ImageFilter();
                var result = imagefilter.CheckAndResizeImage(filePathAndName, upload.InputStream);

                var material = materialRepository.GetElement(UserId.Value, id);
                if (!string.IsNullOrEmpty(material.Filename))
                {
                    //delete previous image
                    var absPath = FileHelper.GetMaterialFilePathAndName(material.Filename, UserId.Value.ToString());
                    if (System.IO.File.Exists(absPath))
                    {
                        log.InfoFormat("Try to delete previous file {0}.", material.Filename);
                        System.IO.File.Delete(absPath);
                    }
                }
                materialRepository.SaveImage(UserId.Value, id, Path.GetFileName(filePathAndName));
                log.InfoFormat("New material image file {0} saved.", material.Filename);
            }
            catch (Exception exc)
            {
                log.Error("Import error.", exc);
                return new FineUploaderResult(false, error: exc.Message);
            }
            return new FineUploaderResult(true, new { fileName = filePathAndName });
        }
        //
        // GET: /Material/Delete/5

        public ActionResult Delete(int id)
        {
            MaterialView material = materialRepository.GetElement(UserId, id);
            string resultMsg;
            if (material != null)
            {
                if (materialRepository.CanDelete(id))
                {
                    materialRepository.Delete(UserId.Value, id);
                    resultMsg = String.Format("<span style='color: red'>{0} would have been deleted.</span>", material.Name);
                }
                else
                {
                    resultMsg = string.Format("Es sind noch Einheiten vorhanden.");
                }
            }
            else
            {
                resultMsg = "<span style='color: red'>Material was not found.</span>";
            }
            if (Request.IsAjaxRequest())
            {
                return Content(resultMsg);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeleteImage(int id)
        {
            MaterialView material = materialRepository.GetElement(UserId, id);
            string resultMsg;
            if (material != null)
            {
                materialRepository.SaveImage(UserId.Value, id, string.Empty);
                resultMsg = String.Format("<span style='color: red'>{0} would have been deleted.</span>", material.Name);
            }
            else
            {
                resultMsg = "<span style='color: red'>Material was not found.</span>";
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new {result = true});
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        public ActionResult Image(int id, string tempId)
        {
            var material = materialRepository.GetElement(UserId, id);
            var subPath = Path.Combine(UserId.Value.ToString(), "Material");
            var filePathAndName = string.Empty;
            if (material != null && !string.IsNullOrEmpty(material.Filename))
            {
                filePathAndName = FileHelper.GetAttachmentFilePathAndName(material.Filename, subPath);
                if (System.IO.File.Exists(filePathAndName))
                {
                    var extension = System.IO.Path.GetExtension(filePathAndName);
                    var contentType = "image/png";
                    if (extension.ToLower() == ".jpg")
                    {
                        contentType = "image/jpeg";
                    }
                    return base.File(filePathAndName, contentType);
                }
            }
            filePathAndName = Server.MapPath("~/Content/images/transparent.png");
            return base.File(filePathAndName, "image/png");
        }
    }
}
