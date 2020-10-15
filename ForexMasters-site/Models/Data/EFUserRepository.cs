using ForexMasters_site.Models.Entities;

namespace ForexMasters_site.Models.Data
{
    public class EFUserRepository : RepositoryBase<User>, IUserRepository
    {
        public EFUserRepository(AppDbContext appDbContext)
            : base(appDbContext) { }
    }
}
