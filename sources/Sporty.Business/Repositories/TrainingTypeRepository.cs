using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Repositories
{
    public class TrainingTypeRepository : BaseRepository<TrainingType>, ITrainingTypeRepository
    {
        public TrainingTypeRepository(SportyEntities context) : base(context)
        {
        }

        #region ITrainingTypeRepository Members

        public IEnumerable<TrainingTypeView> GetAll(Guid userId)
        {
            IQueryable<TrainingType> trainingTypeList = context.TrainingType.Where(t => t.UserId == userId);
            return trainingTypeList.Select(trainingType => new TrainingTypeView
                                                               {
                                                                   Name = trainingType.Name,
                                                                   Id = trainingType.Id
                                                               }).ToList();
        }

        public void CheckAndUpdateTrainingTypes(Guid userId, List<TrainingTypeView> trainingTypesViews)
        {
            IQueryable<TrainingType> trainingTypeList = context.TrainingType.Where(s => s.UserId == userId);

            var typesToRemove = new List<TrainingType>();

            foreach (TrainingType type in trainingTypeList)
            {
                TrainingTypeView sv = trainingTypesViews.SingleOrDefault(v => v.Id == type.Id);
                if (sv == null)
                {
                    //check auf referenzen
                    if (type.Exercise.Count() == 0 && type.Plan.Count() == 0)
                        typesToRemove.Add(type);
                }
            }

            foreach (TrainingType typeToRemove in typesToRemove)
            {
                context.TrainingType.Remove(typeToRemove);

                try
                {
                    Update();
                }
                catch (SqlException sqlException)
                {
                    //sammle Message das Sportart nicht gelöscht werden kann
                }
            }

            foreach (TrainingTypeView trainingTypeView in trainingTypesViews)
            {
                if (trainingTypeView.Id == 0)
                {
                    this.context.TrainingType.Add(new TrainingType {Name = trainingTypeView.Name, UserId = userId});
                }
                else
                {
                    TrainingTypeView view = trainingTypeView;
                    TrainingType sp = trainingTypeList.SingleOrDefault(s => s.Id == view.Id);
                    if (sp != null && (sp.Name != trainingTypeView.Name))
                    {
                        sp.Name = trainingTypeView.Name;
                        //sp.Type = (int)sportTypeView.Type;
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