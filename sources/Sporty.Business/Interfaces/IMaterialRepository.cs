using Sporty.DataModel;
using Sporty.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sporty.Business.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        IEnumerable<MaterialView> GetMaterials(Guid? userId);
        MaterialView GetElement(Guid? userId, int id);
        void Save(Guid userId, MaterialView element);
        void SaveImage(Guid userId, int materialId, string filename);
        void Delete(Guid userId, int id);
        IEnumerable<MaterialView> GetMaterialPerExercise(Guid? userId, int exerciseId);
        //void SaveMaterialPerExercise(Guid? UserId, int p, IEnumerable<int> enumerable);
        bool CanDelete(int id);
        void UpdateMaterialPerExercise(int exerciseId, IEnumerable<int> selectedMaterialIds, Guid? userId);
    }
}
