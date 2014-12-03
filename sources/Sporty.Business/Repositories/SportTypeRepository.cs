using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Repositories
{
    public class SportTypeRepository : BaseRepository<SportType>, ISportTypeRepository
    {
        public SportTypeRepository(SportyEntities context)
            : base(context)
        {
        }

        #region ISportTypeRepository Members

        public IEnumerable<SportTypeView> GetAll(Guid userId)
        {
            IQueryable<SportType> sportTypeList = context.SportType.Where(s => s.UserId == userId);
            return sportTypeList.Select(sportType => new SportTypeView
                                                         {
                                                             Name = sportType.Name,
                                                             Id = sportType.Id,
                                                             Discipline = (Disciplines) sportType.Type
                                                         }).ToList();
        }

        public void CheckAndUpdateSportTypes(Guid userId, List<SportTypeView> sportTypesViews)
        {
            IQueryable<SportType> sportTypeList = context.SportType.Where(s => s.UserId == userId);

            var typesToRemove = new List<SportType>();

            foreach (SportType type in sportTypeList)
            {
                SportTypeView sv = sportTypesViews.SingleOrDefault(v => v.Id == type.Id);
                if (sv == null)
                {
                    //check auf referenzen
                    if (type.Exercise.Count() == 0 && type.Plan.Count() == 0)
                        typesToRemove.Add(type);
                }
            }

            foreach (SportType typeToRemove in typesToRemove)
            {
                context.SportType.Remove(typeToRemove);

                try
                {
                    Update();
                }
                catch (SqlException sqlException)
                {
                    //sammle Message das Sportart nicht gelöscht werden kann
                }
            }

            foreach (SportTypeView sportTypeView in sportTypesViews)
            {
                if (sportTypeView.Id == 0)
                {
                    this.context.SportType.Add(new SportType
                            {Name = sportTypeView.Name, Type = (int) sportTypeView.Discipline, UserId = userId});
                }
                else
                {
                    SportTypeView view = sportTypeView;
                    SportType sp = sportTypeList.SingleOrDefault(s => s.Id == view.Id);
                    if (sp != null && (sp.Name != sportTypeView.Name || sp.Type != (int) sportTypeView.Discipline))
                    {
                        sp.Name = sportTypeView.Name;
                        sp.Type = (int) sportTypeView.Discipline;
                        
                    }
                }

            }
            try
            {
                Update();
            }
            catch (SqlException sqlException)
            {
                //sammle Meldungen
            }
        }

        #endregion
    }
}