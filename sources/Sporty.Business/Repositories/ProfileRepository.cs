using System;
using System.Linq;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sporty.Business.Repositories
{
    public class ProfileRepository : BaseRepository<Profile>, IProfileRepository
    {
        public ProfileRepository(SportyEntities context)
            : base(context)
        {
        }

        #region IProfileRepository Members

        public Profile GetProfile(Guid userId)
        {
            return this.context.Profile.FirstOrDefault(p => p.UserId == userId);
        }

        public void SaveProfile(UserProfileView profileView)
        {
            Profile profile = GetProfile(profileView.UserId);
            if (profile == null)
            {
                profile = new Profile();
            }

            profile.BodyHeight = profileView.BodyHeight;
            profile.MaxHeartrate = profileView.MaxHeartrate;
            if (profileView.SendMetricsMail && profileView.DailyMetricsMailSendingTime.HasValue)
            { //timespan korrigieren 00:10:20 => 10:20:00
                if (profileView.DailyMetricsMailSendingTime.Value.Hours == 0)
                {
                    var ts = new TimeSpan(profileView.DailyMetricsMailSendingTime.Value.Minutes, profileView.DailyMetricsMailSendingTime.Value.Seconds, 0);
                    profile.DailyMetricsMailSendingTime = ts;
                }
                else
                {
                    profile.DailyMetricsMailSendingTime = profileView.DailyMetricsMailSendingTime;
                }
            }
            else
            {
                profile.DailyMetricsMailSendingTime = null;
            }

            if (profile.ProfileId == Guid.Empty)
            {
                profile.UserId = profileView.UserId;
                profile.ProfileId = Guid.NewGuid();
                this.Add(profile);
            }
            else
            {
                Update();
            }
        }

        public IEnumerable<Profile> GetProfiles(Expression<Func<Profile, bool>> exp)
        {
            return this.context.Profile.Where(exp);
        }

        #endregion
    }
}