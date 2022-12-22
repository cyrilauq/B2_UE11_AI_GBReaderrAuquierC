namespace GBReaderAuquierC.Domains
{
    public struct BookMetaData
    {
        private readonly Dictionary<BookAttribute, string> _attributes = new();

        public BookMetaData(string title, string resume, string isbn, string author)
        {
            _attributes.Add(BookAttribute.Title, title);
            _attributes.Add(BookAttribute.Isbn, isbn);
            _attributes.Add(BookAttribute.Resume, resume);
            _attributes.Add(BookAttribute.Author, author);
        }

        public string this[BookAttribute attr]
        {
            get
            {
                
                if (!_attributes.ContainsKey(attr))
                {
                    throw new ArgumentException($"The field {attr} doesn't exist.");
                }

                return _attributes[attr];
            }
        }
    }
}