using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Repositories
{
    public class PhaseRepository : BaseRepository<Phase>, IPhaseRepository
    {
        public PhaseRepository(SportyEntities context)
            : base(context)
        {
        }

        #region IPhaseRepository Members

        public IEnumerable<PhaseView> GetAll(Guid userId)
        {
            IOrderedQueryable<Phase> phaseList = context.Phase.Where(p => p.UserId == userId).OrderBy(p => p.Order);
            return phaseList.Select(phase => new PhaseView
                                                 {
                                                     ShortName = phase.ShortName,
                                                     Id = phase.Id,
                                                     Description = phase.Description,
                                                     Order = phase.Order
                                                 }).ToList();
        }

        public void CheckAndUpdatePhases(Guid userId, List<PhaseView> phases)
        {
            IQueryable<Phase> phaseList = context.Phase.Where(s => s.UserId == userId);

            var typesToRemove = new List<Phase>();

            foreach (Phase type in phaseList)
            {
                PhaseView sv = phases.SingleOrDefault(v => v.Id == type.Id);
                if (sv == null)
                {
                    //check auf referenzen
                    if (type.WeekPlan.Count() == 0)
                        typesToRemove.Add(type);
                }
            }

            foreach (Phase typeToRemove in typesToRemove)
            {
                context.Phase.Remove(typeToRemove);

                try
                {
                    Update();
                }
                catch (SqlException sqlException)
                {
                    //sammle Message das Sportart nicht gelöscht werden kann
                }
            }

            foreach (PhaseView phasesView in phases)
            {
                if (phasesView.Id == 0)
                {
                    this.context.Phase.Add(new Phase
                            {
                                ShortName = phasesView.ShortName,
                                UserId = userId,
                                Color = phasesView.Color,
                                Description = phasesView.Description,
                                Order = phasesView.Order
                            });
                }
                else
                {
                    PhaseView view = phasesView;
                    Phase sp = phaseList.SingleOrDefault(s => s.Id == view.Id);
                    if (sp != null)
                    {
                        sp.ShortName = phasesView.ShortName;
                        sp.Order = phasesView.Order;
                        sp.Color = phasesView.Color;

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