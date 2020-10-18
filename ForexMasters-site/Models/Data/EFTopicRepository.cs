using ForexMasters_site.Models.Entities;

namespace ForexMasters_site.Models.Data
{
    public class EFTopicRepository : RepositoryBase<Topic>, ITopicRepository
    {
        public EFTopicRepository(AppDbContext appDbContext)
            : base(appDbContext)
        {
        }

    }
}
