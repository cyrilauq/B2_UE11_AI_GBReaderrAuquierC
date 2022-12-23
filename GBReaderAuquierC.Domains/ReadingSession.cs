namespace GBReaderAuquierC.Domains
{
    public class ReadingSession
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
        
        public static ReadingSession operator +(ReadingSession s, int b)
        {
            s.History.Add(b);
            s.LastUpdate = DateTime.Now;
            return s;
        }

        public ReadingSession()
        {
            _begin = DateTime.Now;
            _lastUpdate = DateTime.Now;
        }

        public void Add(int nPage)
        {
            _history.Add(nPage);
        }

        public static ReadingSession Get(DateTime begin, DateTime lastUpdate, IList<int> history)
        {
            ReadingSession result = new();
            result.History = history;
            result.LastUpdate = lastUpdate;
            result.Begin = begin;
            return result;
        }

        public override string ToString()
            => $"Debut: {_begin}, LastEdit: {_lastUpdate}, Historique: {_history.ToString()}";

        public override bool Equals(object? obj)
        {
            if(obj == this) { return true; }
            if(obj?.GetType() != GetType()) { return false; }

            var that = obj as ReadingSession;
            return that._begin.ToString().Equals(_begin.ToString()) 
                && that._lastUpdate.ToString().Equals(_lastUpdate.ToString())
                && _history.SequenceEqual(that._history);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}