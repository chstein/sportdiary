using Sporty.DataModel;

namespace Sporty.Business.Interfaces
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        void Update();
    }
}