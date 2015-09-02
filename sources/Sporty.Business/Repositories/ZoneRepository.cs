using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Repositories
{
    public class ZoneRepository : BaseRepository<Zone>, IZoneRepository
    {
        public ZoneRepository(SportyEntities context)
            : base(context)
        {
        }

        #region IZoneRepository Members

        public IEnumerable<ZoneView> GetAll(Guid userId)
        {
            IQueryable<Zone> zoneList = context.Zone.Where(z => z.UserId == userId);
            return zoneList.Select(z => new ZoneView
                                            {
                                                Name = z.Name,
                                                Id = z.Id,
                                                Color = z.Color
                                            }).ToList();
        }

        public void CheckAndUpdateZones(Guid userId, List<ZoneView> zoneViews)
        {
            IQueryable<Zone> zoneList = context.Zone.Where(s => s.UserId == userId);

            var typesToRemove = new List<Zone>();

            foreach (Zone type in zoneList)
            {
                ZoneView sv = zoneViews.SingleOrDefault(v => v.Id == type.Id);
                if (sv == null)
                {
                    //check auf referenzen
                    if (type.Exercise.Count() == 0 && type.Plan.Count() == 0)
                        typesToRemove.Add(type);
                }
            }

            foreach (Zone typeToRemove in typesToRemove)
            {
                context.Zone.Remove(typeToRemove);

                try
                {
                    Update();
                }
                catch (SqlException sqlException)
                {
                    //sammle Message das Sportart nicht gelöscht werden kann
                }
            }

            foreach (ZoneView zoneView in zoneViews)
            {
                if (zoneView.Id == 0)
                {
                    this.context.Zone.Add(new Zone {Name = zoneView.Name, UserId = userId, Color = zoneView.Color});
                }
                else
                {
                    ZoneView view = zoneView;
                    Zone sp = zoneList.SingleOrDefault(s => s.Id == view.Id);
                    if (sp != null && (sp.Name != zoneView.Name || sp.Color != zoneView.Color))
                    {
                        sp.Name = zoneView.Name;
                        sp.Color = zoneView.Color;
                        try
                        {
                            Update();
                        }
                        catch (SqlException sqlException)
                        {
                            //sammle Meldungen
                        }
                    }
                }
            }
        }

        #endregion
    }
}