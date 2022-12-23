namespace GBReaderAuquierC.Infrastructures
{
    public class PageDto
    {
        private readonly string _content;
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
        
        public PageDto(string content)
        {
            _content = content;
        }
    }
}