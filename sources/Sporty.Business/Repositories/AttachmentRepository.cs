using Sporty.Business.Interfaces;
using Sporty.DataModel;

namespace Sporty.Business.Repositories
{
    public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(SportyEntities context) : base(context)
        {
        }
    }
}