namespace ForexMasters_site.Models.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private AppDbContext _appDbContext;
        private IVideoRepository _video;
        private ICategoryRepository _category;
        private IDocumentRepository _document;
        private IFlashcardRepository _flashcard;
        private IUserRepository _userRepository;
        private ITopicRepository _topicRepository;

        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IVideoRepository Video
        {
            get
            {
                if (_video == null)
                {
                    _video = new EFVideoRepository(_appDbContext);
                }

                return _video;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new EFCategoryRepository(_appDbContext);
                }

                return _category;
            }
        }

        public IDocumentRepository Document
        {
            get
            {
                if (_document == null)
                {
                    _document = new EFDocumentRepository(_appDbContext);
                }

                return _document;
            }
        }

        public IFlashcardRepository Flashcard
        {
            get
            {
                if (_flashcard == null)
                {
                    _flashcard  = new EFFlashcardRepository(_appDbContext);
                }

                return _flashcard;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new EFUserRepository(_appDbContext);
                }

                return _userRepository;
            }
        }

        public IUserRepository Topic
        {
            get
            {
                if (_topicRepository == null)
                {
                    _topicRepository = new EFTopicRepository(_appDbContext);
                }

                return _topicRepository;
            }
        }
    }
}
