using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IApplicationRepository applicationRepository;
        private readonly IProfileRepository profileRepository;

        public UserRepository(SportyEntities context, IApplicationRepository applicationRepository,
                              IProfileRepository profileRepository)
            : base(context)
        {
            this.applicationRepository = applicationRepository;
            this.profileRepository = profileRepository;
        }

        public UserRepository(SportyEntities context)
            : base(context)
        {
        }

        #region IUserRepository Members

        public bool ValidateUser(string username, string password)
        {
            //var user = context.Users.Single(u => u.Name == username && u.Password == password);
            User user = this.context.User.FirstOrDefault(u => u.Name == username && u.Password == password);
            return user != null;
        }

        public Guid? FindUserId(string username)
        {
            User user = this.context.User.FirstOrDefault(u => u.Name == username);
            if (user != null)
                return user.UserId;
            return null;
        }


        public string ApplicationName { get; set; }

        public User CreateUser(string username, string password, string salt, string email, Guid activationCode, string ssoToken)
        {
            var checkExistingUser = this.GetUser(username);
            if (checkExistingUser != null)
                return null;

            Guid appId = applicationRepository.GetApplicationIdOrCreate(ApplicationName);
            var user = new User
                           {
                               ApplicationId = appId,
                               Name = username,
                               Password = password,
                               PasswordSalt = salt,
                               Email = email,
                               LoweredEmail = email.ToLower(),
                               //PasswordQuestion = passwordQuestion,
                               //PasswordAnswer = passwordAnswer,
                               //IsApproved = isApproved,
                               UserId = Guid.NewGuid(),
                               CreateDate = DateTime.Now,
                               LastLockoutDate = DateTime.Now,
                               LastLoginDate = DateTime.Now,
                               LastPasswordChangedDate = DateTime.Now,
                               FailedPasswordAnswerAttemptWindowStart = DateTime.Now,
                               FailedPasswordAttemptWindowStart = DateTime.Now,
                               ActivationCode = activationCode,
                               SsoToken = ssoToken
                           };
            this.Add(user);
            return user;
        }

        public bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
                                                    string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string username, string newPassword, string salt)
        {
            User user = GetUser(username);
            user.Password = newPassword;
            user.PasswordSalt = salt;
            Update();
            return true;
        }

        public string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public void TouchUser(User user)
        {
            user.LastLoginDate = DateTime.Now;
            Update();
        }

        public User GetUser(Expression<Func<User, bool>> expression)
        {
            return this.context.User.FirstOrDefault(expression);
        }

        public User GetUser(string username)
        {
            return this.context.User.FirstOrDefault(u => u.Name == username);
        }

        public User GetUser(Guid id)
        {
            return this.context.User.FirstOrDefault(u => u.UserId == id);
        }

        public List<User> GetAllUsers()
        {
            return this.context.User.ToList();
        }


        public User Approve(Guid activationCode)
        {
            User user = this.context.User.FirstOrDefault(u => u.ActivationCode == activationCode);
            if (user != null)
            {
                user.IsApproved = true;
                Update();
                return user;
            }
            return null;
        }

        public UserProfileView GetUserProfile(Guid userId)
        {
            User user = GetUser(userId);
            Profile profile = profileRepository.GetProfile(userId);
            var userProfileView = new UserProfileView
                                      {
                                          Email = user.Email,
                                          CreateDate = user.CreateDate,
                                          Name = user.Name,
                                          UserId = userId
                                      };
            if (profile != null)
            {
                userProfileView.BodyHeight = profile.BodyHeight;
                userProfileView.MaxHeartrate = profile.MaxHeartrate;
                userProfileView.SendMetricsMail = profile.DailyMetricsMailSendingTime.HasValue;
                userProfileView.DailyMetricsMailSendingTime = profile.DailyMetricsMailSendingTime;
            }

            return userProfileView;
        }


        public Guid? FindUserIdByToken(string token)
        {
            if (String.IsNullOrEmpty(token))
                return null;

            Guid userToken;

            if (Guid.TryParse(token, out userToken))
            {
                string tokenText = userToken.ToString().ToLower();
                User user = this.context.User.Single(u => u.SsoToken.ToLower() == tokenText);
                return user.UserId;
            }
            return null;
        }

        #endregion

        public void SaveProfile(UserProfileView profileView)
        {
            User user = GetUser(profileView.UserId);
            user.Email = profileView.Email;
            user.Name = profileView.Name;
            Update();
        }


        public string GetSsoToken(Guid? userId)
        {
            var user = GetUser(userId.Value);
            return user.SsoToken;
        }
    }
}