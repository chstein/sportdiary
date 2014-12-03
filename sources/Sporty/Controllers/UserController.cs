using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using Sporty.Business.Interfaces;
using Sporty.Infrastructure;
using Sporty.ViewModel;

namespace Sporty.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private IPhaseRepository phaseRepository;
        private IProfileRepository profileRepository;
        private ISportTypeRepository sportTypeRepository;
        private ITrainingTypeRepository trainingTypeRepository;
        private IUserRepository userRepository;
        private IZoneRepository zoneRepository;
        private SportyAccountProvider sportyProvider;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            zoneRepository = ServiceFactory.Current.Resolve<IZoneRepository>();
            trainingTypeRepository = ServiceFactory.Current.Resolve<ITrainingTypeRepository>();
            sportTypeRepository = ServiceFactory.Current.Resolve<ISportTypeRepository>();
            phaseRepository = ServiceFactory.Current.Resolve<IPhaseRepository>();

            userRepository = ServiceFactory.Current.Resolve<IUserRepository>();
            profileRepository = ServiceFactory.Current.Resolve<IProfileRepository>();
            sportyProvider = ServiceFactory.Current.Resolve<SportyAccountProvider>();
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit()
        {
            var profileView = new UserProfileView();

            if (GetUserId().HasValue)
            {
                profileView = UserRepository.GetUserProfile(UserId.Value);
                profileView.SportTypes = new List<SportTypeView>(sportTypeRepository.GetAll(UserId.Value));
                profileView.TrainingTypes = new List<TrainingTypeView>(trainingTypeRepository.GetAll(UserId.Value));
                profileView.Zones = new List<ZoneView>(zoneRepository.GetAll(UserId.Value));
                profileView.Phases = new List<PhaseView>(phaseRepository.GetAll(UserId.Value));
            }

            profileView.AllZones = GetAllZonesForSelect();
            profileView.AllDisciplines = GetAllDisciplinesForSelect();

            Session["ProfileView"] = profileView;
            return View(profileView);
        }

        private Dictionary<Disciplines, string> GetAllDisciplinesForSelect()
        {
            return new Dictionary<Disciplines, string>
                       {
                           {Disciplines.Running, "Run"},
                           {Disciplines.Biking, "Cycle"},
                           {Disciplines.Swimming, "Swimming"},
                           {Disciplines.Other, "Other"}
                       };
        }

        private Dictionary<string, string> GetAllZonesForSelect()
        {
            return new Dictionary<string, string>
                       {
                           {"#FFA500", "orange"},
                           {"#0066FF", "blau"},
                           {"#008B00", "grün"},
                           {"#FF0000", "rot"},
                           {"#FFD700", "gelb"},
                           {"#FF1493", "pink"},
                           {"#5F4632", "braun"},
                           {"#808080", "grau"},
                           {"#4B0082", "lila"}
                       };
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(UserProfileView profileView)
        {
            if (!ModelState.IsValid)
            {
                //Model zurückgeben
            }

            try
            {
                //Update Profile
                var savedProfileView = Session["ProfileView"] as UserProfileView;
                profileView.CreateDate = savedProfileView.CreateDate;

                //Change Password
                if (!String.IsNullOrEmpty(profileView.OldPassword))
                {
                    if (!ValidateChangePassword(profileView.OldPassword, profileView.NewPassword,
                                                profileView.NewPasswordRepeat))
                    {
                        return View(profileView);
                    }

                    try
                    {
                        if (sportyProvider.ChangePassword(User.Identity.Name, profileView.OldPassword,
                                                                                     profileView.NewPassword))
                        {
                            //return RedirectToAction("ChangePasswordSuccess");
                        }
                        else
                        {
                            ModelState.AddModelError("_FORM",
                                                     "The current password is incorrect or the new password is invalid.");
                            return View(profileView);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("_FORM",
                                                 "The current password is incorrect or the new password is invalid.");
                        return View(profileView);
                    }
                }

                //Update User defined types
                if (profileView.SportTypes != null && profileView.SportTypes.Count > 0)
                {
                    sportTypeRepository.CheckAndUpdateSportTypes(GetUserId().Value, profileView.SportTypes);
                }

                if (profileView.Zones != null && profileView.Zones.Count > 0)
                {
                    zoneRepository.CheckAndUpdateZones(GetUserId().Value, profileView.Zones);
                }

                if (profileView.TrainingTypes != null && profileView.TrainingTypes.Count > 0)
                {
                    trainingTypeRepository.CheckAndUpdateTrainingTypes(GetUserId().Value, profileView.TrainingTypes);
                }

                if (profileView.Phases != null && profileView.Phases.Count > 0)
                {
                    phaseRepository.CheckAndUpdatePhases(GetUserId().Value, profileView.Phases);
                }


                profileRepository.SaveProfile(profileView);

                return Json(new { Message = "Daten wurden erfolgreich gespeichert." });
            }
            catch (Exception e)
            {
                return View(profileView);
            }
        }

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", "You must specify a current password.");
            }
            //if (newPassword == null || newPassword.Length < SportyAccountProvider.MinPasswordLength)
            //{
            //    ModelState.AddModelError("newPassword",
            //                             String.Format(CultureInfo.CurrentCulture,
            //                                           "You must specify a new password of {0} or more characters.",
            //                                           SportyAccountProvider.MinPasswordLength));
            //}

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
            }

            return ModelState.IsValid;
        }
    }
}