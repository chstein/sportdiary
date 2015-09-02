using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.Infrastructure;
using Sporty.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sporty.Controllers
{
    public class SampleCreator
    {
        private IExerciseRepository exerciseRepository;
        private ISportTypeRepository sportTypeRepository;
        private ITrainingTypeRepository trainingTypeRepository;
        private IWeekPlanRepository weekPlanRepository;
        private IZoneRepository zoneRepository;
        private IMaterialRepository materialRepository;
        private IUserRepository userRepository;

        public SampleCreator()
        {
            exerciseRepository = ServiceFactory.Current.Resolve<IExerciseRepository>();
            sportTypeRepository = ServiceFactory.Current.Resolve<ISportTypeRepository>();
            zoneRepository = ServiceFactory.Current.Resolve<IZoneRepository>();
            trainingTypeRepository = ServiceFactory.Current.Resolve<ITrainingTypeRepository>();
            weekPlanRepository = ServiceFactory.Current.Resolve<IWeekPlanRepository>();
            materialRepository = ServiceFactory.Current.Resolve<IMaterialRepository>();
            userRepository = ServiceFactory.Current.Resolve<IUserRepository>();
        }

        public void CreateExercise(Guid userId)
        {
            //create default running sporttype
            SportType running = CreateBaseSportType(userId);

            //create zone
            Zone z2 = CreateBaseZones(userId);

            TrainingType training = CreateBaseTrainingTypes(userId);

            Material material = CreateBaseMaterial(userId);

            ExerciseDetails ex = new ExerciseDetails
            {
                Duration = new TimeSpan(1, 0, 0),
                Distance = 10,
                Date = DateTime.Now,
                Description = "Beispieleinheit für eine lockere Laufrunde. Einfach über einen manuellen Eintrag erzeugt.",
                Heartrate = 140,
                SportTypeId = running.Id,
                ZoneId = z2.Id,
                TrainingTypeId = training.Id,
                SelectedMaterialIds = new List<int>{ material.Id}
            };

            int eId = exerciseRepository.Save(userId, ex);
        }

        private Material CreateBaseMaterial(Guid userId)
        {
            Material shoe = new Material
            {
                Name = "Mein Lieblingsschuh",
                Description = "Läuft und läuft und läuft",
                InUsage = true,
                Lifetime = 800,
                UserId = userId
            };
            materialRepository.Add(shoe);
            return shoe;
        }

        private TrainingType CreateBaseTrainingTypes(Guid userId)
        {
            var t = trainingTypeRepository.GetElement(tt => tt.UserId == userId);
            if (t != null)
            {
                return t;
            }
            TrainingType training = new TrainingType
            {
                Name = "Training",
                UserId = userId
            };
            TrainingType comp = new TrainingType
            {
                Name = "Wettkampf",
                UserId = userId
            };
            trainingTypeRepository.Add(training);
            trainingTypeRepository.Add(comp);
            return training;
        }

        private Zone CreateBaseZones(Guid userId)
        {
            var z = zoneRepository.GetElement(zone => zone.UserId == userId);
            if (z != null)
            {
                return z;
            }
            Zone z1 = new Zone
            {
                Name = "Erholung",
                UserId = userId,
                Color = "#FFA500"
            };

            Zone z2 = new Zone
            {
                Name = "Grundlage 1",
                UserId = userId,
                Color = "#FFA500"
            };
            Zone z3 = new Zone
            {
                Name = "Grundlage 2",
                UserId = userId,
                Color = "#FFA500"
            };
            Zone z4 = new Zone
            {
                Name = "Laktatschwelle",
                UserId = userId,
                Color = "#FFA500"
            };
            Zone z5 = new Zone
            {
                Name = "Anaerob",
                UserId = userId,
                Color = "#FFA500"
            };
            zoneRepository.Add(z1);
            zoneRepository.Add(z2);
            zoneRepository.Add(z3);
            zoneRepository.Add(z4);
            zoneRepository.Add(z5);
            return z2;
        }

        private SportType CreateBaseSportType(Guid userId)
        {
            var s = sportTypeRepository.GetElement(st => st.UserId == userId);
            if (s != null)
            {
                return s;
            }

            SportType running = new SportType
            {
                Name = "Laufen",
                Type = (int)Disciplines.Running,
                UserId = userId
            };
            sportTypeRepository.Add(running);
            return running;
        }

        internal static void CreateExerciseWithAttachment()
        {
            throw new NotImplementedException();
        }
    }
}
