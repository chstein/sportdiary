//using System.Web.Security;
//using Sporty.Infrastructure;

//namespace Sporty.Controllers
//{
//    public class AccountMembershipService : IMembershipService
//    {
//        private readonly SportyAccountProvider _provider;

//        public AccountMembershipService()
//            : this(null)
//        {
//        }

//        public AccountMembershipService(MembershipProvider provider)
//        {
//            _provider = new SportyAccountProvider();
//           // _provider = provider as SportyMembershipProvider ?? Membership.Provider as SportyMembershipProvider;
//        }

//        #region IMembershipService Members

//        public bool ValidateUser(string userName, string password)
//        {
//            return _provider.ValidateUser(userName, password);
//        }

//        public bool ChangePassword(string userName, string oldPassword, string newPassword)
//        {
//            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
//            return currentUser.ChangePassword(oldPassword, newPassword);
//        }


//        public MembershipCreateStatus CreateUser(string userName, string password, string email,
//                                                 string absoluteTemplatePath, string applicationServerPath)
//        {
//            MembershipCreateStatus status;
//            _provider.CreateUser(userName, password, email, null, null, false, null, out status, absoluteTemplatePath,
//                                 applicationServerPath);
//            return status;
//        }

//        #endregion
//    }
//}