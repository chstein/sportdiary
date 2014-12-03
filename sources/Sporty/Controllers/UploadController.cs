using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sporty.Business.IO;
using Sporty.Business.Interfaces;
using Sporty.Common;
using Sporty.DataModel;
using Sporty.Helper;
using Sporty.Infrastructure;
using Common.Logging;

namespace Sporty.Controllers
{
    [UploadAuthorize]
    public class UploadController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UploadController));

        private IExerciseRepository exerciseRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (UserId.HasValue)
            {
                exerciseRepository = ServiceFactory.Current.Resolve<IExerciseRepository>();
            }
        }

        public JsonResult ExerciseUpload(string qqfile)
        {
            var fileName = Request.QueryString["qqfile"];
            string extension = Path.GetExtension(fileName).ToLower();
            var stream = Request.InputStream;
            string text = string.Empty;
            try
            {
                var sr = new StreamReader(stream);
                text = sr.ReadToEnd();

            }
            catch (Exception exc)
            {
                log.Error("Upload error.", exc);
                return Json(new { success = false });
            }
            
            if (Request.Browser.Browser == "IE")
            {
                var files = System.Web.HttpContext.Current.Request.Files;
                var postedFile = files[0];
                if (postedFile.FileName != "")
                {
                    var fn = Path.GetFileName(postedFile.FileName);
                    fileName = fn;
                }
            }

            if (exerciseRepository == null)
            {
                exerciseRepository = ServiceFactory.Current.Resolve<IExerciseRepository>();
            }
            var newExerciseId = 0;
            try
            {
                if (extension == ".tur" || extension == ".hrm" || extension == ".gpx" || extension == ".tcx")
                {
                    string filePathAndName = FileHelper.SaveFile(text, fileName, extension, UserId.Value.ToString());

                    newExerciseId = ImportFile(filePathAndName, extension);
                }
            }
            catch (Exception exc)
            {
                log.Error("Import error.", exc);
            }

            return Json(new { success = true, id = newExerciseId });
        }

        public JsonResult AttachmentUpload(int? id, string qqfile)
        {
            var fileName = Request.QueryString["qqfile"];

            if (exerciseRepository == null)
                exerciseRepository = ServiceFactory.Current.Resolve<IExerciseRepository>();
            int attachmentId = 0;
            string filePathAndName = String.Empty;
            try
            {
                attachmentId = AttachToExercise(id, fileName);
            }
            catch (Exception exception)
            {
                log.Error("Attachment upload error.", exception);
            }

            return Json(new { Id = attachmentId, Name = Path.GetFileName(fileName), Success = true });
        }

        private int AttachToExercise(int? id, string fileName)
        {
            Exercise exercise = exerciseRepository.GetElement(e => e.UserId == GetUserId() && e.Id == id.Value);
            var attachment = new Attachment
                                 {
                                     Filename = Path.GetFileName(fileName)
                                 };
            exercise.Attachment.Add(attachment);
            exerciseRepository.Update();
            return attachment.Id;
        }

        private int ImportFile(string filePathAndName, string extension)
        {
            //var geoCalculator = ServiceFactory.Current.Resolve<IGeoCalculator>();

            ExerciseParser parser = ExerciseParser.GetParser(extension);

            Exercise exercise = parser.ParseExercise(filePathAndName);

            //try to get existing exercise by date and hours and minutes of duration
            //Exercise existingEx = exerciseRepository.GetElement(e => e.Date.Date == exercise.Date.Date &&
            //                                                         (e.Duration.HasValue && exercise.Duration.HasValue &&
            //                                                          e.Duration.Value.Hours ==
            //                                                          exercise.Duration.Value.Hours &&
            //                                                          e.Duration.Value.Minutes ==
            //                                                          exercise.Duration.Value.Minutes));

            ////overwrite exercise
            //if (existingEx != null && existingEx.Id > 0)
            //    exercise.Id = existingEx.Id;

            exercise.UserId = UserId;

            if (exercise.SportType != null)
            {
                SportType sp = exerciseRepository.GetSportType(UserId, exercise.SportType.Type);
                exercise.SportType = sp;
            }
            else
            {
                exercise.SportTypeId = 1;
            }
            var attachment = new Attachment
                                 {
                                     Filename = Path.GetFileName(filePathAndName)
                                 };

            exercise.Attachment.Add(attachment);

            exerciseRepository.Add(exercise);

            return exercise.Id;
        }

        
    }
}