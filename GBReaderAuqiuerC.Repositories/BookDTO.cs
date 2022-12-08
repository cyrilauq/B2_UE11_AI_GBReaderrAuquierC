namespace GBReaderAuquierC.Repositories
{
    public record BookDTO
    {
        private int _id;
        private readonly string _title;
        private readonly string _author;
        private readonly string _isbn;
        private readonly string _version;
        private readonly string _resume;
        private readonly string _imagePath;
        private IList<PageDTO> _pages = new List<PageDTO>();

        public string Version { get => _version; }
        
        public string Title { get => _title; }
        public string Author { get => _author; }
        public string ISBN { get => _isbn; }
        public string Resume { get => _resume; }
        public string ImagePath { get => _imagePath; }
        public IList<PageDTO> Pages { get => _pages; set => _pages = value; }

        public int Id { get => _id; set => _id = value; }

        public BookDTO(string title, string resume, string author, string isbn, string imgPath, string version = "1.1")
        {
            _title = title;
            _author = author;
            _resume = resume;
            _isbn = isbn;
            _version = version;
            _imagePath = imgPath;
        }

        public BookDTO(string title, string resume, string author, string isbn, string imgPath, IList<PageDTO> pages, string version = "1.1")
        {
            _title = title;
            _author = author;
            _resume = resume;
            _isbn = isbn;
            _version = version;
            _imagePath = imgPath;
            _pages = pages;
        }
        
        public override string ToString()
        {
            return $"{Title}, {ISBN}, {Resume}, {Author}";
        }
    }
}