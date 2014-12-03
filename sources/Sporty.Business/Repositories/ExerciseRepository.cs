using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Sporty.Business.Helper;
using Sporty.Business.IO;
using Sporty.Business.Interfaces;
using Sporty.Business.Series;
using Sporty.DataModel;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;
using Sporty.Common;
using System.Data.Entity;

namespace Sporty.Business.Repositories
{
    public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
    {
        private readonly CalcHelper calcHelper;
        private readonly IGoalRepository goalRepository;
        private readonly IMetricRepository metricRepository;
        private readonly IPhaseRepository phaseRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IWeekPlanRepository weekPlanRepository;
        private readonly IMaterialRepository materialRepository;

        public ExerciseRepository(SportyEntities context, IGoalRepository goalRepository,
                                  IWeekPlanRepository weekPlanRepository,
                                  IPhaseRepository phaseRepository, IMetricRepository metricRepository,
                                  IProfileRepository profileRepository, IMaterialRepository materialRepository)
            : base(context)
        {
            this.goalRepository = goalRepository;
            this.weekPlanRepository = weekPlanRepository;
            this.phaseRepository = phaseRepository;
            this.metricRepository = metricRepository;
            this.profileRepository = profileRepository;
            this.materialRepository = materialRepository;
            calcHelper = new CalcHelper(profileRepository, metricRepository, this);
        }

        #region IExerciseRepository Members

        public int Save(Guid? userId, ExerciseDetails element)
        {
            Exercise exercise = element.Id > 0
                                    ? this.context.Exercise.SingleOrDefault(e => e.Id == element.Id && e.UserId == userId)
                                    : new Exercise { Id = element.Id };

            exercise.SportType = context.SportType.First(s => s.Id == element.SportTypeId);

            exercise.DateLocal = element.Date;
            exercise.Description = element.Description;
            exercise.Distance = element.Distance;
            exercise.Duration = element.Duration;
            exercise.Heartrate = element.Heartrate;
            exercise.HeartrateMax = element.HeartrateMax;
            exercise.Speed = element.Speed;
            exercise.SpeedMax = element.SpeedMax;
            exercise.Cadence = element.Cadence;
            exercise.CadenceMax = element.CadenceMax;

            if (element.TrainingTypeId.HasValue)
                exercise.TrainingType = context.TrainingType.First(tt => tt.Id == element.TrainingTypeId);
            if (element.ZoneId.HasValue)
                exercise.Zone = context.Zone.First(z => z.Id == element.ZoneId);
            if (!string.IsNullOrEmpty(element.SelectedWeatherCondition))
                exercise.WeatherCondition = element.SelectedWeatherCondition.ToLower();
            else
                exercise.WeatherCondition = string.Empty;
            exercise.WeatherNote = element.WeatherNote;
            exercise.Temperature = element.Temperature;

            exercise.UserId = userId;

            exercise.Trimp = calcHelper.CalculateTrimp(exercise);

            if (exercise.Id > 0)
            {
                base.Update();
            }
            else
            {
                context.Exercise.Add(exercise);
            }

            UpdateMaterialPerExercise(exercise.Id, element.SelectedMaterialIds, userId);
            UpdateAttachmentsPerExercise(exercise.Id, element.Attachments, userId);
            context.SaveChanges();//.SubmitChanges();
            return exercise.Id;
        }

        private void UpdateAttachmentsPerExercise(int exerciseId, IEnumerable<AttachmentView> attachments, Guid? userId)
        {
            if (attachments == null)
                attachments = new List<AttachmentView>();
            var currentAttachmentsPerExercise = context.Attachment.Where(a => a.ExerciseId == exerciseId);

            var itemsToAdd = attachments.Where(selectedAttachments => !currentAttachmentsPerExercise.Select(a => a.Id).Contains(selectedAttachments.Id)).ToList();

            var newAttachments = itemsToAdd.Select(item => new Attachment { ExerciseId = exerciseId, Filename = item.Filename }).ToList();
            foreach (var at in newAttachments)
            {
                context.Attachment.Add(at);
            }
            context.SaveChanges();//.Attachment.InsertAllOnSubmit(newAttachments);
        }

        public void UpdateMaterialPerExercise(int exerciseId, IEnumerable<int> selectedMaterialIds, Guid? userId)
        {
            //wenn nix da, dann können alle Einträge gelöscht werden (im Zuge der Delete Abfrage weiter unten)
            if (selectedMaterialIds == null) selectedMaterialIds = new List<int>();

            var currentMaterialPerExercise = context.MaterialPerExercise.Where(m => m.ExerciseId == exerciseId);

            var itemsToRemove = (from materialPerExercise in currentMaterialPerExercise
                                 where
                                     !selectedMaterialIds.Contains(materialPerExercise.MaterialId)
                                 select materialPerExercise.Id).ToList();

            foreach (var item in itemsToRemove.Select(itemToRemove => context.MaterialPerExercise.Single(mpe => mpe.Id == itemToRemove)))
            {
                context.MaterialPerExercise.Remove(item);
                //context.MaterialPerExercise.DeleteOnSubmit(item);
            }
            var itemsToAdd = selectedMaterialIds.Where(selectedMaterialId => !currentMaterialPerExercise.Select(mpe => mpe.MaterialId).Contains(selectedMaterialId)).ToList();

            var newMpe = itemsToAdd.Select(item => new MaterialPerExercise { ExerciseId = exerciseId, MaterialId = item }).ToList();
            foreach (var mpe in newMpe)
            {
                context.MaterialPerExercise.Add(mpe);
            }
            context.SaveChanges();//.MaterialPerExercise..InsertAllOnSubmit(newMpe);
        }

        public ExerciseDetails GetElement(Guid? userId, int id)
        {
            Exercise exercise = this.context.Exercise.SingleOrDefault(e => e.Id == id && e.UserId == userId);
            if (exercise == null) return null;

            var detailsView = new ExerciseDetails
                                  {
                                      SportTypeId = exercise.SportTypeId,
                                      SportTypeName =
                                          exercise.SportType != null ? exercise.SportType.Name : String.Empty,
                                      Date = exercise.DateLocal,
                                      Description = exercise.Description,
                                      Distance = exercise.Distance,
                                      Duration = exercise.Duration,
                                      Heartrate = exercise.Heartrate,
                                      Speed = exercise.Speed,
                                      SpeedMax = exercise.SpeedMax,
                                      HeartrateMax = exercise.HeartrateMax,
                                      Cadence = exercise.Cadence,
                                      CadenceMax = exercise.CadenceMax,
                                      TrainingTypeId = exercise.TrainingTypeId,
                                      TrainingTypeName =
                                          exercise.TrainingType != null ? exercise.TrainingType.Name : String.Empty,
                                      ZoneId = exercise.ZoneId,
                                      ZoneName = exercise.Zone != null ? exercise.Zone.Name : String.Empty,
                                      Id = exercise.Id,
                                      SelectedWeatherCondition = !string.IsNullOrEmpty(exercise.WeatherCondition) ? exercise.WeatherCondition.ToLower() : String.Empty,
                                      Temperature = exercise.Temperature,
                                      WeatherNote = exercise.WeatherNote,
                                      Trimp =
                                          exercise.Trimp.HasValue
                                              ? exercise.Trimp.Value
                                              : calcHelper.CalculateTrimp(exercise)
                                  };

            if (detailsView.Distance.HasValue && detailsView.Distance.Value != 0 &&
                detailsView.Duration.HasValue && detailsView.Duration.Value.TotalHours != 0)
            {
                detailsView.Speed = detailsView.Distance.Value / detailsView.Duration.Value.TotalHours;
                detailsView.Pace = detailsView.Duration.Value.TotalMinutes / detailsView.Distance.Value;
            }
            detailsView.SelectedMaterialIds = exercise.MaterialPerExercise.Select(mpe => mpe.MaterialId);

            if (exercise.Attachment.Count > 0)
            {
                detailsView.Attachments =
                    exercise.Attachment.Select(item => new AttachmentView { Filename = item.Filename, Id = item.Id }).
                        ToList();
            }

            return detailsView;
            //return context.Exercises.Where(e => e.Id == id && e.User.Id == userId).Single();
        }

        public ExerciseDetailsView GetDetailsView(Guid? userId, int id)
        {
            Exercise exercise = this.context.Exercise.FirstOrDefault(e => e.Id == id && e.User.UserId == userId);
            if (exercise == null) return null;

            var detailsView = new ExerciseDetailsView
            {
                SportTypeId = exercise.SportTypeId,
                SportTypeName =
                    exercise.SportType != null ? exercise.SportType.Name : String.Empty,
                Date = exercise.DateLocal,
                Description = exercise.Description,
                Distance = exercise.Distance,
                Duration = exercise.Duration,
                Heartrate = exercise.Heartrate,
                HeartrateMax = exercise.HeartrateMax,
                Speed = exercise.Speed,
                SpeedMax = exercise.SpeedMax,
                TrainingTypeId = exercise.TrainingTypeId,
                TrainingTypeName =
                    exercise.TrainingType != null ? exercise.TrainingType.Name : String.Empty,
                ZoneId = exercise.ZoneId,
                ZoneName = exercise.Zone != null ? exercise.Zone.Name : String.Empty,
                Id = exercise.Id,
                Cadence = exercise.Cadence,
                CadenceMax = exercise.CadenceMax,
                Trimp =
                    exercise.Trimp.HasValue
                        ? exercise.Trimp.Value
                        : calcHelper.CalculateTrimp(exercise)
            };

            if (detailsView.Distance.HasValue && detailsView.Distance.Value != 0 &&
                detailsView.Duration.HasValue && detailsView.Duration.Value.TotalHours != 0)
            {
                detailsView.Speed = detailsView.Distance.Value / detailsView.Duration.Value.TotalHours;
                detailsView.Pace = detailsView.Duration.Value.TotalMinutes / detailsView.Distance.Value;
            }

            detailsView.SelectedMaterials = exercise.MaterialPerExercise.Select(mpe => mpe.MaterialId);

            if (exercise.Attachment.Count > 0)
            {
                detailsView.Attachments =
                    exercise.Attachment.Select(item => new AttachmentView { Filename = item.Filename, Id = item.Id }).
                        ToList();
            }

            return detailsView;
            //return context.Exercises.Where(e => e.Id == id && e.User.Id == userId).Single();
        }

        public void Delete(Guid? userId, int id)
        {
            //delete all attachments
            Exercise ex = this.context.Exercise.FirstOrDefault(e => e.Id == id && e.UserId == userId);
            if (ex.Attachment != null && ex.Attachment.Count > 0)
            {
                var attToDelete = new List<Attachment>();
                foreach (var att in ex.Attachment)
                {
                    attToDelete.Add(att);
                }
                foreach (var attachment in attToDelete)
                {
                    context.Attachment.Remove(attachment);
                }
            }
            if (ex.MaterialPerExercise != null && ex.MaterialPerExercise.Count > 0)
            {
                var mpeToDelete = new List<MaterialPerExercise>();
                foreach (MaterialPerExercise material in ex.MaterialPerExercise)
                {
                    mpeToDelete.Add(material);
                }
                foreach (var item in mpeToDelete)
                {
                    context.MaterialPerExercise.Remove(item);
                }
            }
            
            this.context.Exercise.Remove(ex);
            this.context.SaveChanges();
        }

        public IEnumerable<ExerciseView> GetExercises(Guid? userId)
        {
            if (!userId.HasValue) return null;
            IQueryable<Exercise> exerciseList = context.Exercise.Where(e => e.UserId == userId);
            return GetExerciseViewList(exerciseList);
        }

        public AttachmentView GetAttachment(Guid? userId, int attachmentId)
        {
            if (!userId.HasValue) return null;
            Attachment attachment = context.Attachment.FirstOrDefault(a => a.Id == attachmentId);
            return attachment != null ? new AttachmentView { Filename = attachment.Filename, Id = attachment.Id } : null;
        }

        public void DeleteAttachment(Guid? userId, int attachmentId)
        {
            if (!userId.HasValue) return;
            Attachment attachment = context.Attachment.FirstOrDefault(a => a.Id == attachmentId);
            context.Attachment.Remove(attachment);

            Update();
        }

        public List<ExercisesPerTimeUnit> GetReportExercises(Guid? userId, DateTime fromLocalDate, DateTime toLocalDate,
                                                             TimeUnit timeUnit, ReportTypeName reportTypeName)
        {
            ReportSeries series = null;
            IEnumerable<ExerciseView> exercises;
            int daysDiff;
            DateTime currentUtcDate;
            IQueryable<string> sportTypeNames;
            List<ExercisesPerTimeUnit> exercisesPerDay = new List<ExercisesPerTimeUnit>();

            if (reportTypeName == ReportTypeName.Training)
            {
                //fürs berechnen der Werte muss "zurückgeblickt" werden
                var startDate = fromLocalDate.Subtract(new TimeSpan(TrainingAllSportTypeSeries.LongTerm, 0, 0, 0));
                exercises = GetExercises(userId, startDate, toLocalDate);
                //daysDiff bleibt auf dem originalen Wert (da darüber die Anzeige gesteuert wird
                daysDiff = toLocalDate.Subtract(fromLocalDate).Days;
                currentUtcDate = DateTimeConverter.GetUtcDateTime(startDate, User.LocalTimeZone);
                sportTypeNames =
                    context.SportType.Where(s => s.UserId == userId.Value).Select(s => s.Name);
                series = new TrainingAllSportTypeSeries(calcHelper);
            }
            else
            {
                //var utcToDate = DateTime.SpecifyKind(toDate.Date, DateTimeKind.Utc);
                //var utcFromDate = DateTime.SpecifyKind(fromDate.Date, DateTimeKind.Utc);
                exercises = GetExercises(userId, fromLocalDate, toLocalDate);
                daysDiff = toLocalDate.Subtract(fromLocalDate).Days;
                currentUtcDate = DateTimeConverter.GetUtcDateTime(fromLocalDate, User.LocalTimeZone);
                sportTypeNames =
                    context.SportType.Where(s => s.UserId == userId.Value).Select(s => s.Name);
                switch (reportTypeName)
                {
                    case ReportTypeName.DurationPerSportType:
                        series = new DurationPerSportTypeSeries();
                        break;
                    case ReportTypeName.HeartratePerSportType:
                        series = new HeartratePerSportTypeSeries();
                        break;

                    case ReportTypeName.DurationSummary:
                        series = new DurationAllSportTypeSeries();
                        break;
                    case ReportTypeName.HeartrateSummary:
                        series = new HeartrateAllSportTypeSeries();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("reportTypeName");
                }
            }
            return series.GetSeries(timeUnit, exercisesPerDay, sportTypeNames, currentUtcDate, daysDiff, exercises);
        }

        public ExerciseView RefreshFromAttachment(int exerciseId, int attachmentId, string rootFolder, Guid? userId)
        {
            Attachment attachment = context.Attachment.FirstOrDefault(a => a.Id == attachmentId);
            ExerciseParser parser = ExerciseParser.GetParser(Path.GetExtension(attachment.Filename));
            Exercise oldExercise = this.context.Exercise.FirstOrDefault(e => e.UserId == userId && e.Id == exerciseId);

            if (rootFolder.Last() != Path.DirectorySeparatorChar)
            {
                rootFolder = String.Concat(rootFolder, Path.DirectorySeparatorChar);
            }
            string filePathAndName = String.Concat(rootFolder, userId, @"\", attachment.Filename);

            Exercise exercise = parser.ParseExercise(filePathAndName, oldExercise);
            return GetExerciseView(exercise);
        }

        public SportType GetSportType(Guid? userId, int type)
        {
            if (!userId.HasValue) return null;
            return context.SportType.FirstOrDefault(s => s.UserId == userId && s.Type == type);
        }

        public IEnumerable<ExerciseView> GetExercises(Guid? userId, int month, int year)
        {
            if (!userId.HasValue) return null;
            IQueryable<Exercise> exerciseList =
                context.Exercise.Where(e => e.Date.Month == month && e.Date.Year == year && e.UserId == userId);
            return GetExerciseViewList(exerciseList);
        }

        public IEnumerable<ExerciseView> GetPagedExercise(Guid? userId, int itemsPerPage, int? page,
                                                          string sortColumnName, SortDirection sortDirection)
        {
            if (!userId.HasValue) return null;
            if (!page.HasValue) page = 1;
            //var List = base.GetPagedItems(userId, itemsPerPage, page.Value, sortColumnName, sortDirection);


            int start = (page.Value * itemsPerPage) - itemsPerPage;
            int end = page.Value * itemsPerPage;
            //context.GetTable<TEntity>().OrderBy()
            IEnumerable<Exercise> table = context.Exercise.Where(e => e.UserId == userId);
            if (!String.IsNullOrEmpty(sortColumnName))
                table = base.OrderBy<Exercise>(table, sortColumnName, sortDirection);
            else
                table = base.OrderBy<Exercise>(table, "Date", SortDirection.Descending);

            List<Exercise> exerciseList = table.Skip(start).Take(end).ToList();
            if (exerciseList.Count == 0)
                return null;

            return GetExerciseViewList(exerciseList);
        }

        public IEnumerable<ExerciseView> GetExercises(Guid? userId, DateTime fromLocalDate, DateTime? toLocalDate)
        {
            if (!userId.HasValue) return null;

            DateTime fromUtc = DateTimeConverter.GetUtcDateTime(fromLocalDate, User.LocalTimeZone);
            DateTime? toUtc = null;
            if (toLocalDate.HasValue)
            {
                toUtc = DateTimeConverter.GetUtcDateTime(toLocalDate.Value, User.LocalTimeZone);
            }
            IQueryable<Exercise> exercises = toUtc.HasValue
                                                 ? context.Exercise.Where(
                                                     e => e.Date >= fromUtc && e.Date <= toUtc && e.UserId == userId)
                                                 : context.Exercise.Where(
                                                     e => e.Date >= fromUtc && e.UserId == userId);

            return GetExerciseViewList(exercises);
        }

        public IEnumerable<Exercise> GetExercises(Expression<Func<Exercise, bool>> expression)
        {
            //context.Exercise.Include(e => e.SportType);
            return context.Exercise.Include(e => e.SportType).Where(expression);
        }

        #endregion

        private IEnumerable<ExerciseView> GetExerciseViewList(IEnumerable<Exercise> exerciseList)
        {
            return exerciseList.Select(GetExerciseView).ToList();
        }

        private ExerciseView GetExerciseView(Exercise exercise)
        {
            return new ExerciseView
                       {
                           Id = exercise.Id,
                           Date = DateTimeConverter.GetLocalDateTime(exercise.Date, User.LocalTimeZone),
                           Distance = exercise.Distance,
                           Duration = exercise.Duration,
                           SportTypeName =
                               exercise.SportType != null
                                   ? exercise.SportType.Name
                                   : String.Empty,
                           TrainingTypeName =
                               exercise.TrainingType != null
                                   ? exercise.TrainingType.Name
                                   : String.Empty,
                           ZoneName =
                               exercise.Zone != null ? exercise.Zone.Name : String.Empty,
                           Description = exercise.Description,
                           Heartrate = exercise.Heartrate,
                           HasAttachment = exercise.Attachment.Any(),
                           Speed = exercise.Speed,
                           Cadence = exercise.Cadence,
                           HeartrateMax = exercise.HeartrateMax,
                           CadenceMax = exercise.CadenceMax,
                           SpeedMax = exercise.SpeedMax,
                           Trimp = exercise.Trimp.HasValue ? exercise.Trimp : calcHelper.CalculateTrimp(exercise)
                       };
        }


        public bool CanDeleteAttachment(string attachmentFilename)
        {
            var attachments = context.Attachment.Where(a => a.Filename == attachmentFilename);
            if (attachments != null && attachments.Count() > 1)
                return false;
            return true;
        }


        public Exercise GetElement(int id)
        {
            return this.context.Exercise.SingleOrDefault(e => e.Id == id);
        }


        public void Add(Exercise exercise)
        {
            this.context.Exercise.Add(exercise);
            this.context.SaveChanges();
        }
    }
}