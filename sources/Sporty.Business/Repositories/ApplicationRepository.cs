using System;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using System.Linq;

namespace Sporty.Business.Repositories
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(SportyEntities context)
            : base(context)
        {
        }

        #region IApplicationRepository Members

        public Guid GetApplicationIdOrCreate(string applicationName)
        {
            Application app = this.context.Application.FirstOrDefault(a => a.ApplicationName == applicationName);
            if (app == null)
            {
                app = new Application
                          {
                              ApplicationId = Guid.NewGuid(),
                              ApplicationName = applicationName,
                              LoweredApplicationName = applicationName.ToLower()
                          };
                this.context.Application.Add(app);
            }
            return app.ApplicationId;
        }

        #endregion
    }
}