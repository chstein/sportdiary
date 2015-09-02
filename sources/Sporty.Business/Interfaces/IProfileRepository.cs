using System;
using Sporty.DataModel;
using Sporty.ViewModel;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sporty.Business.Interfaces
{
    public interface IProfileRepository
    {
        Profile GetProfile(Guid userId);
        void SaveProfile(UserProfileView profileView);
        IEnumerable<Profile> GetProfiles(Expression<Func<Profile, bool>> exp);
    }
}