using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Sporty.DataModel;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Interfaces
{
    public interface IExerciseRepository : IRepository<Exercise>
    {
        IEnumerable<ExerciseView> GetExercises(Guid? userId, int month, int year);

        IEnumerable<ExerciseView> GetPagedExercise(Guid? userId, int itemsPerPage, int? page, string sortColumnName,
                                                   SortDirection sortDirection);

        //List<ExercisesPerTimeUnit> GetReportExercises(Guid? userId, int month, int year, bool displayOnlyData);

        Exercise GetElement(int id);
        
        ExerciseDetails GetElement(Guid? userId, int id);
        int Save(Guid? userId, ExerciseDetails element);
        void Delete(Guid? userId, int id);
        IEnumerable<ExerciseView> GetExercises(Guid? userId);
        IEnumerable<Exercise> GetExercises(Expression<Func<Exercise, bool>> expression);
        void Update();
        AttachmentView GetAttachment(Guid? userId, int attachmentId);
        void DeleteAttachment(Guid? userId, int attachmentId);

        List<ExercisesPerTimeUnit> GetReportExercises(Guid? userId, DateTime fromDate, DateTime toDate,
                                                      TimeUnit timeUnit, ReportTypeName reportTypeName);

        ExerciseView RefreshFromAttachment(int exerciseId, int attachmenId, string rootFolder, Guid? userId);
        SportType GetSportType(Guid? userId, int type);
        IEnumerable<ExerciseView> GetExercises(Guid? userId, DateTime fromDate, DateTime? toDate);
        ExerciseDetailsView GetDetailsView(Guid? userId, int id);

        bool CanDeleteAttachment(string attachmentFilename);

        void Add(Exercise exercise);
    }
}