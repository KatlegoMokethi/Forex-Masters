using ForexMasters_site.Models.Entities;

namespace ForexMasters_site.Models.Data
{
    public class EFCategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public EFCategoryRepository(AppDbContext appDbContext)
            : base(appDbContext)
        {
        }

    }
}
