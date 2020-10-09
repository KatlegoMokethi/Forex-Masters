using ForexMasters_site.Models.Entities;

namespace ForexMasters_site.Models.Data
{
    public class EFFlashcardRepository : RepositoryBase<Flashcard>, IFlashcardRepository
    {
        public EFFlashcardRepository(AppDbContext appDbContext)
            : base(appDbContext)
        {
        }

    }
}
