using System;
using Sporty.DataModel;

namespace Sporty.Business.Interfaces
{
    public interface IApplicationRepository : IRepository<Application>
    {
        Guid GetApplicationIdOrCreate(string applicationName);
    }
}