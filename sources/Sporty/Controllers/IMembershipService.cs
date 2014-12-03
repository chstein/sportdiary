using System.Web.Security;

namespace Sporty.Controllers
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);

        MembershipCreateStatus CreateUser(string userName, string password, string email, string absoluteTemplatePath,
                                          string applicationServerPath);
    }
}