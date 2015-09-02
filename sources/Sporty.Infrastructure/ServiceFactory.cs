using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sporty.Business;
using Sporty.Business.IO;
using Sporty.DataModel;
using Microsoft.Practices.Unity;
using Sporty.Business.Interfaces;
using Sporty.Business.Repositories;
using System.Web.Configuration;
using System.Configuration;
using Sporty.Common;

namespace Sporty.Infrastructure
{
    public class ServiceFactory
    {
        static ServiceFactory factory = new ServiceFactory();

        public static ServiceFactory Current
        {
            get
            {
                return factory;
            }
        }

        ConnectionStringSettings connString;



        /// <summary>
        /// Prevent "before field init" IL annotation
        /// </summary>
        static ServiceFactory() { }
        private IUnityContainer container;


        private ServiceFactory()
        {
            container = new UnityContainer();
            //read connectionstring
            var rootWebConfig = WebConfigurationManager.OpenWebConfiguration("/");
            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connString = rootWebConfig.ConnectionStrings.ConnectionStrings[Constants.ConnectionString];
                if (connString == null)
                    throw new NullReferenceException("Can't read connection string 'ApplicationServices' from config.");
            }
            ConfigureContainer();
        }

        void ConfigureContainer()
        {
            container.RegisterType<IExerciseRepository, ExerciseRepository>(new TransientLifetimeManager());
            container.RegisterType<IProfileRepository, ProfileRepository>(new TransientLifetimeManager());
            container.RegisterType<IPlanRepository, PlanRepository>(new TransientLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new TransientLifetimeManager());
            container.RegisterType<IApplicationRepository, ApplicationRepository>(new TransientLifetimeManager());
            container.RegisterType<IAttachmentRepository, AttachmentRepository>(new TransientLifetimeManager());
            container.RegisterType<ISportTypeRepository, SportTypeRepository>(new TransientLifetimeManager());
            container.RegisterType<IZoneRepository, ZoneRepository>(new TransientLifetimeManager());
            container.RegisterType<ITrainingTypeRepository, TrainingTypeRepository>(new TransientLifetimeManager());
            container.RegisterType<IGoalRepository, GoalRepository>(new TransientLifetimeManager());
            container.RegisterType<IMetricRepository, MetricRepository>(new TransientLifetimeManager());
            container.RegisterType<IMaterialRepository, MaterialRepository>(new TransientLifetimeManager());
            //container.RegisterType<IGeoCalculator, GeoCalculator>(new TransientLifetimeManager());
            container.RegisterType<IPhaseRepository, PhaseRepository>(new TransientLifetimeManager());
            container.RegisterType<IWeekPlanRepository, WeekPlanRepository>(new TransientLifetimeManager());
            container.RegisterType<ICalendarService, CalendarService>(new TransientLifetimeManager());

            var injectedConnectionString = new InjectionConstructor(connString.ConnectionString);
            container.RegisterType<SportyEntities, SportyEntities>(new TransientLifetimeManager(), injectedConnectionString);

        }

        public TService Resolve<TService>()
        {
            return container.Resolve<TService>();
        }


    }
}

