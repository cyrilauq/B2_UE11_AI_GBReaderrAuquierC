namespace GBReaderAuquierC.Domains
{
    public class Page
    {
        private Dictionary<string, Page> _choices = new();
        private string _content;

        public string Content
        {
            get => _content;
        }

        public Page(string content)
        {
            _content = content;
        }

        public void addChoice(string label, Page target)
        {
            if (label != null && target != null)
            {
                _choices[label] = target;
            }
        }
    }
}