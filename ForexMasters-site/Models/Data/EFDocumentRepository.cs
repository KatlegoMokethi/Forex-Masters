using ForexMasters_site.Models.Entities;

namespace ForexMasters_site.Models.Data
{
    public class EFDocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public EFDocumentRepository(AppDbContext appDbContext)
            : base(appDbContext)
        {
        }

    }
}
