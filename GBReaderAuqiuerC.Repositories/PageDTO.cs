namespace GBReaderAuquierC.Repositories
{
    public class PageDTO
    {
        private string _content;
        private Dictionary<string, string> _choices = new();

        public Dictionary<string, string> Choices
        {
            get => _choices;
            set => _choices = value;
        }

        public string Content
        {
            get => _content;
        }
        
        public PageDTO(string content)
        {
            _content = content;
        }
    }
}