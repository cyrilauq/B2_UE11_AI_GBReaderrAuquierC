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

        public Dictionary<string, Page> Choices
        {
            set => _choices = value;
            get => new(_choices);
        }
        
        public bool HasChoices { get => _choices.Count > 0; }
        
        public bool IsTerminal { get => _choices.Count == 0; }

        public Page(string content)
        {
            _content = content;
        }

        public Page GetPageFor(string content)
        {
            return _choices[content];
        }

        public void AddChoice(string label, Page target)
        {
            if (label != null && target != null)
            {
                _choices[label] = target;
            }
        }

        public override bool Equals(object? obj)
        {
            if(this == obj) { return true; }
            if(this.GetType() != obj.GetType()) { return false; }
            Page that = obj as Page;
            return this._content.Equals(that._content);
        }

        public override string ToString() => $"{nameof(_choices)}: {_choices}, {nameof(_content)}: {_content}";
    }
}