using System.Data.Linq;

namespace Sporty.DataModel
{
    public interface IEntitiesDataContext
    {
        Table<Attachment> Attachments { get; }
        Table<Application> Applications { get; }
        Table<User> Users { get; }
        Table<Profile> Profiles { get; }
        Table<Exercise> Exercises { get; }
        Table<Plan> Plans { get; }
        Table<SportType> SportTypes { get; }
        Table<TrainingType> TrainingTypes { get; }
        Table<Zone> Zones { get; }
        Table<Goal> Goals { get; }
        Table<Metric> Metrics { get; }
        Table<Phase> Phases { get; }
        Table<WeekPlan> WeekPlans { get; }
        Table<Material> Materials { get; }
        Table<MaterialPerExercise> MaterialPerExercises { get; }

        void SubmitChanges();

        Table<TEntity> GetTable<TEntity>()
            where TEntity : class;


    }
}