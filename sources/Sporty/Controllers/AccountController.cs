using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Principal;
using System.Web.Security;
using Sporty.Business.Interfaces;
using Sporty.Infrastructure;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using Sporty.Models;
using System.Web.Mvc;

namespace Sporty.Controllers
{
    [HandleError]
    [Authorize]
    public class AccountController : Controller
    {
        private IUserRepository userRepository;


        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
            : this(null, null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, SportyAccountProvider sportyAccountProvider)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            SportyAccountProvider = sportyAccountProvider ?? new SportyAccountProvider();
        }

        public IFormsAuthentication FormsAuth { get; private set; }

        public SportyAccountProvider SportyAccountProvider { get; set; }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && SportyAccountProvider.ValidateUser(model.UserName, model.Password))
            {
                FormsAuth.SignIn(model.UserName, model.RememberMe);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Der Benutzername / Passwort ist nicht gültig.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Activate(Guid? id)
        {
            if (id.HasValue)
            {
                userRepository = ServiceFactory.Current.Resolve<IUserRepository>();

                var user = userRepository.Approve(id.Value);
                if (user != null)
                {
                    //TODO Initiale EInheiten usw. anlegen für den Einstieg
                    var sv = new SampleCreator();
                    sv.CreateExercise(user.UserId);
                    FormsAuth.SignIn(user.Name, false /* createPersistentCookie */);
                    return RedirectToAction("Calendar", "Exercise");
                }
            }
            return RedirectToAction("Error", "Home");
        }

        [AllowAnonymous]
        public ActionResult RegisteredSuccess()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mailTemplatePath = Server.MapPath(@"/templates/activation_mail.html");
                    MembershipCreateStatus createStatus = SportyAccountProvider.CreateUser(model, mailTemplatePath, GetAppServer());

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        //TODO Information for need to activate Account per Mail
                        return RedirectToAction("RegisteredSuccess", "Account");
                    }
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    //return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        protected string GetAppServer()
        {
            String path = Request.Url.OriginalString;
            path = path.Substring(0, path.LastIndexOf("/") + 1);
            return path;
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mailTemplatePath = Server.MapPath(@"/templates/reset_password.html");

                    var result = SportyAccountProvider.ResetPassword(model.Email, mailTemplatePath);
                    if (result)
                    {
                        return View("ResetPasswordSuccess");
                    }
                    else
                    {
                        model.Message = "Die E-Mail Adresse wurde nicht gefunden.";
                        return View(model);
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Validation Methods

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Der Benutzername ist bereits vergeben. Bitte versuche einen neuen.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ein Benutzer hat bereits diese Emailadresse. Evtl. das Passwort vergessen?";

                //case MembershipCreateStatus.InvalidPassword:
                //    return "Password nicht gültig.";

                //case MembershipCreateStatus.InvalidEmail:
                //    return "Email ust nic";

                //case MembershipCreateStatus.InvalidAnswer:
                //    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                //case MembershipCreateStatus.InvalidQuestion:
                //    return "The password retrieval question provided is invalid. Please check the value and try again.";

                //case MembershipCreateStatus.InvalidUserName:
                //    return "The user name provided is invalid. Please check the value and try again.";

                //case MembershipCreateStatus.ProviderError:
                //    return
                //        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                //case MembershipCreateStatus.UserRejected:
                //    return
                //        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "Es ist ein Fehler aufgetreten. Bitte noch einmal versuchen. Wenn es wieder auftritt, dann eine kurze Info an mich. Kontaktdetails siehe unten.";
            }
        }

        #endregion

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.
}