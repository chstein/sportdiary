using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sporty.Business.Repositories
{
    public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(SportyEntities context)
            : base(context)
        {
        }

        public IEnumerable<MaterialView> GetMaterials(Guid? userId)
        {
            var materialList = this.context.Material.Where(g => g.UserId == userId);
            //var milage = GetCurrentMilage(item.id);


            var ml = materialList.Select(item => new MaterialView
            {
                Id = item.Id,
                Description = item.Description,
                Name = item.Name,
                InUsage = item.InUsage,
                Lifetime = item.Lifetime,
                //Image fehlt noch
            }).ToList();

            foreach (var m in ml)
            {
                m.Milage = GetCurrentMilage(m.Id);

            }
            return ml;
        }

        private double? GetCurrentMilage(int materialId)
        {
            var exIds = this.context.MaterialPerExercise.Where(mpe => mpe.MaterialId == materialId).Select(mpe => mpe.ExerciseId);
            return exIds.Select(exId => context.Exercise.FirstOrDefault(e => e.Id == exId)).Sum(e => e.Distance);
        }

        public IEnumerable<MaterialView> GetMaterialPerExercise(Guid? userId, int exerciseId)
        {
            var materialPerExerciseList = context.MaterialPerExercise.Where(m => m.ExerciseId == exerciseId);
            var mViewList =
                materialPerExerciseList.Select(mpe => context.Material.Single(m => m.Id == mpe.MaterialId)).Select(
                    material => new MaterialView
                        {
                            Id = material.Id,
                            Description = material.Description,
                            Name = material.Name,
                            InUsage = material.InUsage,
                            Lifetime = material.Lifetime
                            //Image fehlt noch
                        }).ToList();
            return mViewList;
        }

        public void UpdateMaterialPerExercise(int exerciseId, IEnumerable<int> selectedMaterialIds, Guid? userId)
        {
            var currentMaterialPerExercise = context.MaterialPerExercise.Where(m => m.ExerciseId == exerciseId);
            var itemsToRemove = (from materialPerExercise in currentMaterialPerExercise
                                 where
                                     !selectedMaterialIds.Contains(materialPerExercise.MaterialId)
                                 select materialPerExercise.Id).ToList();

            foreach (var item in itemsToRemove.Select(itemToRemove => context.MaterialPerExercise.Single(mpe => mpe.Id == itemToRemove)))
            {
                context.MaterialPerExercise.Remove(item);
            }
            var itemsToAdd = selectedMaterialIds.Where(selectedMaterialId => !currentMaterialPerExercise.Select(mpe => mpe.MaterialId).Contains(selectedMaterialId)).ToList();

            var newMpe = itemsToAdd.Select(item => new MaterialPerExercise { ExerciseId = exerciseId, MaterialId = item }).ToList();

            foreach (var newMp in newMpe)
            {
                context.MaterialPerExercise.Add(newMp);
            }

        }

        public MaterialView GetElement(Guid? userId, int id)
        {
            Material material = this.context.Material.FirstOrDefault(m => m.Id == id && m.UserId == userId);
            if (material == null) return null;

            var materialView = new MaterialView
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description,
                InUsage = material.InUsage,
                Lifetime = material.Lifetime,
                Filename = material.Filename
            };

            return materialView;
        }

        public void Save(Guid userId, MaterialView element)
        {
            Material material = element.Id > 0
                            ? context.Material.FirstOrDefault(e => e.Id == element.Id && e.UserId == userId)
                            : new Material { Id = element.Id };

            material.Name = element.Name;
            material.Description = element.Description;
            material.UserId = userId;
            material.InUsage = element.InUsage;
            material.Lifetime = element.Lifetime;

            if (material.Id > 0)
                base.Update();
            else
                base.Add(material);
        }

        public void SaveImage(Guid userId, int materialId, string filename)
        {
            Material material = context.Material.FirstOrDefault(e => e.Id == materialId && e.UserId == userId);

            material.Filename = filename;

            base.Update();
        }

        public bool CanDelete(int id)
        {
            var ex = context.MaterialPerExercise.Count(e => e.MaterialId == id);
            return (ex == 0);
        }

        public void Delete(Guid userId, int id)
        {
            Delete(g => g.Id == id && g.User.UserId == userId);
        }
    }
}
