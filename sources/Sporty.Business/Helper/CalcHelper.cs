using System;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Helper
{
    public class CalcHelper : ICalcHelper
    {
        private readonly IExerciseRepository exerciseRepository;
        private readonly IMetricRepository metricRepository;
        private readonly IProfileRepository profileRepository;

        public CalcHelper(IProfileRepository profileRepository, IMetricRepository metricRepository,
                          IExerciseRepository exerciseRepository)
        {
            this.profileRepository = profileRepository;
            this.metricRepository = metricRepository;
            this.exerciseRepository = exerciseRepository;
        }

        #region ICalcHelper Members

        public int CalculateTrimp(Exercise exercise)
        {
            if (exercise.UserId == null || !exercise.Duration.HasValue || !exercise.Heartrate.HasValue)
            {
                return 0;
            }
            Profile profile = profileRepository.GetProfile(exercise.UserId.Value);
            if (profile == null || !profile.MaxHeartrate.HasValue)
            {
                return 0;
            }

            var latestMetric = metricRepository.GetLatestMetric(exercise.UserId.Value);
            if (latestMetric == null || !latestMetric.RestingPulse.HasValue)
            {
                return 0;
            }

            double heartrateComponent = (Convert.ToDouble(exercise.Heartrate.Value) -
                                         Convert.ToDouble(latestMetric.RestingPulse.Value))/
                                        (Convert.ToDouble(profile.MaxHeartrate.Value) -
                                         Convert.ToDouble(latestMetric.RestingPulse.Value));

            double genderFactor = 1.92;
            if (profile.IsMale.HasValue && !profile.IsMale.Value)
            {
                genderFactor = 1.67;
            }

            double trimp = exercise.Duration.Value.TotalMinutes*heartrateComponent*0.64*
                           Math.Exp(genderFactor*heartrateComponent);
            //http://fellrnr.com/wiki/TRIMP
            //TRIMPexp = 30 x (130-40)/(200-40) x 0.64e(1.92x(130-40)/(200-40))
            return Convert.ToInt32(trimp);
        }

        public int CalculateTrimp(ExerciseView exerciseView)
        {
            Exercise exc = exerciseRepository.GetElement(exerciseView.Id);
            return CalculateTrimp(exc);
        }

        #endregion
    }
}