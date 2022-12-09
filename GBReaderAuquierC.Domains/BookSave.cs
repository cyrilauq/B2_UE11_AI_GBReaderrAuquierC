namespace GBReaderAuquierC.Domains
{
    public class BookSave
    {
        private DateTime _begin;
        private DateTime _lastUpdate;

        private IList<int> _history = new List<int>();

        public IList<int> History
        {
            get => new List<int>(_history);
            private set => _history = value;
        }

        public DateTime LastUpdate
        {
            get => _lastUpdate;
            private set => _lastUpdate = value;
        }

        public DateTime Begin
        {
            get => _begin;
            private set => _begin = value;
        }
        
        public int Count { get => _history.Count; }

        public int Last { get => _history.Last(); }
        
        public static BookSave operator +(BookSave s, int b)
        {
            s.History.Add(b);
            s.LastUpdate = DateTime.Now;
            return s;
        }

        public BookSave()
        {
            _begin = DateTime.Now;
            _lastUpdate = DateTime.Now;
        }

        public void Add(int nPage)
        {
            _history.Add(nPage);
        }

        public static BookSave Get(DateTime begin, DateTime lastUpdate, IList<int> history)
        {
            BookSave result = new();
            result.History = history;
            result.LastUpdate = lastUpdate;
            result.Begin = begin;
            return result;
        }
    }
}