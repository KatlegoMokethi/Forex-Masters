using ForexMasters_site.Models.Entities;

namespace ForexMasters_site.Models.Data
{
    public class EFVideoRepository : RepositoryBase<Video>, IVideoRepository
    {
        public EFVideoRepository(AppDbContext appDbContext)
            : base(appDbContext)
        {
        }

    }
}
