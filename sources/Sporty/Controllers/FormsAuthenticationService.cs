using System.Web.Security;

namespace Sporty.Controllers
{
    public class FormsAuthenticationService : IFormsAuthentication
    {
        #region IFormsAuthentication Members

        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}