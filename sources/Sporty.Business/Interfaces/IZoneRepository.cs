using System;
using System.Collections.Generic;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface IZoneRepository : IRepository<Zone>
    {
        IEnumerable<ZoneView> GetAll(Guid userId);
        void CheckAndUpdateZones(Guid userId, List<ZoneView> zones);
    }
}