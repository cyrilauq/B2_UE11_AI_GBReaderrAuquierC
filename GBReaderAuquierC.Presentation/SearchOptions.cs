namespace GBReaderAuquierC.Presentation
{
    public class SearchOption
    {
        public static readonly SearchOption FilterTitle = new ("Titre");
        public static readonly SearchOption FilterIsbn = new ("ISBN");
        public static readonly SearchOption NoFilter = new ("Les deux");

        private string _optionName;

        public string OptionName
        {
            get => _optionName;
        }

        private SearchOption(string optionName)
        {
            _optionName = optionName;
        }

        public override bool Equals(object? obj)
        {
            if(obj == this ) { return true; }
            if(obj.GetType() != this.GetType())  { return false; }

            var o = obj as SearchOption;
            return o.OptionName == _optionName;
        }
    }

}