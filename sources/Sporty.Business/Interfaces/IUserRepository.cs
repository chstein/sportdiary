using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        string ApplicationName { get; set; }
        bool ValidateUser(string username, string password);

        User CreateUser(string username, string p, string salt, string email, Guid activationCode, string toString);

        bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
                                             string newPasswordAnswer);

        string GetPassword(string username, string answer);

        bool ChangePassword(string username, string newPassword, string salt);

        string ResetPassword(string username, string answer);

        bool DeleteUser(string username, bool deleteAllRelatedData);

        void TouchUser(User user);

        User GetUser(Expression<Func<User, bool>> expression);

        User GetUser(string username);

        User GetUser(Guid userId);

        List<User> GetAllUsers();

        Guid? FindUserId(string username);

        User Approve(Guid userId);

        UserProfileView GetUserProfile(Guid userId);

        Guid? FindUserIdByToken(string token);

        string GetSsoToken(Guid? UserId);
    }
}