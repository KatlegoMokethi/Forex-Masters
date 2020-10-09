namespace ForexMasters_site.Models.Data
{
    public interface IRepositoryWrapper
    {
        IVideoRepository Video { get; }
        ICategoryRepository Category { get; }
        IDocumentRepository Document { get; }
        IFlashcardRepository Flashcard { get; }
    }
}
