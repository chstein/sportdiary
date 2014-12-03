using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Sporty.Business.Interfaces;
using Sporty.Helper;
using Sporty.Infrastructure;
using Sporty.ViewModel;

namespace Sporty.Controllers
{
    public class ServicesController : Controller
    {
        private IExerciseRepository exerciseRepository;
        private IGoalRepository goalRepository;
        private IPlanRepository planRepository;
        private IUserRepository userRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            goalRepository = ServiceFactory.Current.Resolve<IGoalRepository>();
            userRepository = ServiceFactory.Current.Resolve<IUserRepository>();
            exerciseRepository = ServiceFactory.Current.Resolve<IExerciseRepository>();
            planRepository = ServiceFactory.Current.Resolve<IPlanRepository>();
        }

        //
        // GET: /Services/

        public ActionResult GoalICalFeed(string id)
        {
            //get user by token
            Guid? userId = userRepository.FindUserIdByToken(id);
            if (!userId.HasValue)
            {
                return null;
            }
            
            IEnumerable<GoalView> goals = goalRepository.GetGoals(userId.Value, GetStartDate(), GetEndDate());

            return new GoalCalResult(goals.ToList(), "Goals.ics");
        }

        public ActionResult ExerciseICalFeed(string id)
        {
            //get user by token
            Guid? userId = userRepository.FindUserIdByToken(id);
            if (!userId.HasValue)
            {
                return null;
            }
            
            IEnumerable<ExerciseView> exercises = exerciseRepository.GetExercises(userId, GetStartDate(), GetEndDate());

            return new ExerciseCalResult(exercises.ToList(), "Exercises.ics");
        }

        private static DateTime GetEndDate()
        {
            var today = DateTime.Now;
            
            var endDate = new DateTime(today.Year, 12, 31);
            if (today.Month > 9)
            {
                //wenn ende des Jahres, dann Ziele vom nächsten Jahr auch gleich mitnehmen
                endDate = new DateTime(today.Year + 1, 12, 31);
            }
            return endDate;
        }

        private static DateTime GetStartDate()
        {
            var today = DateTime.Now;
            
            var startDate = new DateTime(today.Year, 1, 1);

            //wenn jan oder feb, dann komplettes letztes Jahr mit
            if (today.Month < 3)
            {
                startDate = new DateTime(today.Year - 1, 1, 1);
            }
            return startDate;
        }

        public ActionResult PlanICalFeed(string id)
        {
            //get user by token
            Guid? userId = userRepository.FindUserIdByToken(id);
            if (!userId.HasValue)
            {
                return null;
            }
            IEnumerable<PlanView> plans = planRepository.GetPlans(userId, GetStartDate(), GetEndDate());

            //if (dinners == null)
            //    return View("NotFound");

            return new PlanCalResult(plans.ToList(), "Plans.ics");
        }
    }
}