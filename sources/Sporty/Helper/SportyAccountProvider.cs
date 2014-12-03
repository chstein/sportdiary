using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Xml.Linq;
using Sporty.DataModel;
using Sporty.Infrastructure;
using Sporty.Business;
using Sporty.Business.Repositories;
using System.Text;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using Sporty.Business.Interfaces;
using System.Net.Mail;
using System.IO;
using Sporty.Common.Mail;
using Sporty.Models;
using Sporty.Helper;

namespace Sporty.Infrastructure
{
    public class SportyAccountProvider// : MembershipProvider
    {
        private IUserRepository repository;
        private MembershipPasswordFormat passwordFormat;

        /// <summary>
        /// Default constructor to call MembershipProvider
        /// </summary>
        public SportyAccountProvider()
        {
            repository = ServiceFactory.Current.Resolve<IUserRepository>();
            ApplicationName = "/";
        }

        public SportyAccountProvider(IUserRepository repository)
        {
            this.repository = repository;
            ApplicationName = "/";
        }

        public string ConnectionString { get; set; }

        private string applicationName;
        public string ApplicationName
        {
            get { return applicationName; }
            set
            {
                applicationName = value;
                repository.ApplicationName = value;
            }
        }

        public MembershipPasswordFormat PasswordFormat
        {
            get { return passwordFormat; }
        }

        public bool ResetPassword(string email, string mailTemplatePath)
        {
            var user = repository.GetUser(u => u.Email == email.ToLower());
            if (user != null)
            {
                var newPassword = GeneratePasswort();
                var newSalt = CreateSalt(32);
                repository.ChangePassword(user.Name, EncodePassword(newPassword, newSalt), newSalt);

                MailHandler.SendResetPasswordMail(user.Name, user.Email, newPassword, mailTemplatePath);
                return true;
            }
            return false;
        }

        private string GeneratePasswort()
        {
            return Membership.GeneratePassword(8, 0);
        }

        public MembershipCreateStatus CreateUser(RegisterModel model, string absoluteTemplatePath, string applicationServerPath)
        {
            if (!string.IsNullOrEmpty(GetUserNameByEmail(model.Email)))
            {
                return MembershipCreateStatus.DuplicateEmail;
            }
            var salt = CreateSalt(32);

            var user = repository.CreateUser(model.UserName, EncodePassword(model.Password, salt), salt,
                                             model.Email, Guid.NewGuid(), Guid.NewGuid().ToString());
            if (user == null)
            {
                return MembershipCreateStatus.DuplicateUserName;
            }
            else
            {
                if (!String.IsNullOrEmpty(applicationServerPath) && !String.IsNullOrEmpty(absoluteTemplatePath))
                    MailHandler.SendActivationMail(user.Name, user.Email, user.ActivationCode.Value.ToString(), absoluteTemplatePath, applicationServerPath);

                return MembershipCreateStatus.Success;
            }
        }



        private MembershipUser GetMembershipUser(User user)
        {
            var now = DateTime.Now;
            return new MembershipUser(typeof(SportyAccountProvider).Name, user.Name, user.UserId,
                                      user.Email, string.Empty, user.Comment, user.IsApproved,
                                      user.IsLockedOut,
                                      user.CreateDate,
                                      user.LastLoginDate,
                                      user.LastLoginDate,
                                      user.LastPasswordChangedDate,
                                      user.LastLockoutDate);
        }

        public bool ChangePasswordQuestionAndAnswer(string username, string password,
                                                             string newPasswordQuestion, string newPasswordAnswer)
        {
            return repository.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
        }

        public string GetPassword(string username, string answer)
        {
            return repository.GetPassword(username, answer);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {

            if (ValidateUser(username, oldPassword))
            {
                var salt = CreateSalt(32);
                return repository.ChangePassword(username, EncodePassword(newPassword, salt), salt);
            }
            return false;
        }

        public void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            //HACK Repository muss upgedated werden, evtl. UnityCode / LifeTime ändern?
            repository = ServiceFactory.Current.Resolve<IUserRepository>();

            User user;
            try
            {
                user = repository.GetUser(username);
                if (user == null)
                {
                    return false;
                }
            }
            catch (ProviderException)
            {
                return false;
            }

            if (CheckPassword(password, user.PasswordSalt, user.Password))
            {
                if (user.IsApproved && !user.IsLockedOut)
                {
                    isValid = true;
                    repository.TouchUser(user);
                }
            }

            return isValid;

        }

        public MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return GetMembershipUser(repository.GetUser((Guid)providerUserKey));
        }

        public MembershipUser GetUser(string username)
        {
            return GetMembershipUser(repository.GetUser(username));
        }

        public string GetUserNameByEmail(string email)
        {
            var user = repository.GetUser(email);
            if (user != null)
                return user.Name;
            return null;
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            bool isCorrect = repository.DeleteUser(username, deleteAllRelatedData);
            return isCorrect;
        }

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var users = repository.GetAllUsers();
            var mUsers = new MembershipUserCollection();
            foreach (var user in users)
            {
                mUsers.Add(GetMembershipUser(user));
            }
            totalRecords = users.Count;
            return mUsers;
        }

        //public override void Initialize(string name, NameValueCollection config)
        //{
        //    // Initialize values from web.config.
        //    if (config == null)
        //    {
        //        throw new ArgumentNullException("config");
        //    }

        //    if (string.IsNullOrEmpty(name))
        //    {
        //        name = "SportyMembershipProvider";
        //    }

        //    if (string.IsNullOrEmpty(config["description"]))
        //    {
        //        config.Remove("description");
        //        config.Add("description", "SportyMembershipProvider");
        //    }

        //    // Initialize the abstract base class.
        //    //base.Initialize(name, config);

        //    ApplicationName =
        //        Convert.ToString(ProviderUtils.GetConfigValue(config, "applicationName",
        //                                                      HostingEnvironment.ApplicationVirtualPath));
        //    minRequiredPasswordLength =
        //        Convert.ToInt32(ProviderUtils.GetConfigValue(config, "minRequiredPasswordLength", "2"));
        //    enablePasswordReset = Convert.ToBoolean(ProviderUtils.GetConfigValue(config, "enablePasswordReset", "true"));
        //    enablePasswordRetrieval =
        //        Convert.ToBoolean(ProviderUtils.GetConfigValue(config, "enablePasswordRetrieval", "false"));
        //    requiresUniqueEmail = Convert.ToBoolean(ProviderUtils.GetConfigValue(config, "requiresUniqueEmail", "true"));

        //    if (!string.IsNullOrEmpty(passwordStrengthRegularExpression))
        //    {
        //        passwordStrengthRegularExpression = passwordStrengthRegularExpression.Trim();
        //        if (!string.IsNullOrEmpty(passwordStrengthRegularExpression))
        //        {
        //            try
        //            {
        //                new Regex(passwordStrengthRegularExpression);
        //            }
        //            catch (ArgumentException ex)
        //            {
        //                throw new ProviderException(ex.Message, ex);
        //            }
        //        }
        //    }

        //    string temp_format = config["passwordFormat"] ?? "Hashed";

        //    switch (temp_format)
        //    {
        //        case "Hashed":
        //            passwordFormat = MembershipPasswordFormat.Hashed;
        //            break;
        //        case "Encrypted":
        //            passwordFormat = MembershipPasswordFormat.Encrypted;
        //            break;
        //        case "Clear":
        //            passwordFormat = MembershipPasswordFormat.Clear;
        //            break;
        //        default:
        //            throw new ProviderException("Password format not supported.");
        //    }

        //    // Get encryption and decryption key information from the configuration.
        //    var configuration =
        //        WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
        //    machineKey = (MachineKeySection)configuration.GetSection("system.web/machineKey");

        //    if (machineKey.ValidationKey.Contains("AutoGenerate"))
        //    {
        //        if (PasswordFormat != MembershipPasswordFormat.Clear)
        //        {
        //            throw new ProviderException(
        //                "Hashed or Encrypted passwords are not supported with auto-generated keys.");
        //        }
        //    }
        //}

        private bool CheckPassword(string password, string salt, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            pass1 = EncodePassword(password, salt);

            return pass1 == pass2;
        }

        /// <summary>
        /// Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string EncodePassword(string password, string salt)
        {
            byte[] bytePASS = Encoding.Unicode.GetBytes(password);
            byte[] byteSALT = Convert.FromBase64String(salt);
            byte[] byteRESULT = new byte[byteSALT.Length + bytePASS.Length + 1];

            System.Buffer.BlockCopy(byteSALT, 0, byteRESULT, 0, byteSALT.Length);
            System.Buffer.BlockCopy(bytePASS, 0, byteRESULT, byteSALT.Length, bytePASS.Length);

            // Create a new instance of the hash crypto service provider.
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider();
            // Compute the Hash. This returns an array of Bytes.
            byte[] bytHash = hashAlg.ComputeHash(byteRESULT);
            // Optionally, represent the hash value as a base64-encoded string, 
            // For example, if you need to display the value or transmit it over a network.
            string encodedPassword = Convert.ToBase64String(bytHash);


            return encodedPassword;
        }

        private string CreateSalt(int size)
        {
            // Generate a cryptographic random number using the cryptographic 
            // service provider
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        private byte[] EncryptPassword(byte[] p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decrypts or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="encodedPassword"></param>
        /// <returns></returns>
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        private byte[] DecryptPassword(byte[] p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        //public bool SaveAllChanges()
        //{
        //    using (var transaction = new TransactionScope())
        //    {
        //        try
        //        {
        //            var context = repository.StoreContext as IUnitOfWork;
        //            context.Commit();
        //            transaction.Complete();
        //            return true;
        //        }
        //        catch (Exception exc)
        //        {
        //            throw new Exception(exc.Message, exc);
        //        }
        //    }
        //}




    }
}
