using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Events;

namespace GBReaderAuquierC.Presentation.Views
{
    public interface IHomeView
    {
        // TODO : Changer les Display par des attributs dans les vue.
        event EventHandler<DescriptionEventArgs> DisplayDetailsRequested;
        event EventHandler ReadBookRequested;
        event EventHandler ViewStatRequested;
        event EventHandler<SearchEventArgs> SearchBookRequested;
        event EventHandler<ChangePageEventArgs> ChangePageRequested;
        
        int ActualPage { set; get; }
        BookExtendedItem BookDetails { set; }
        IEnumerable<BookItem> Books { set; }

        void DisplayMessage(string message);
    }

    public record ChangePageEventArgs(int Move, SearchEventArgs SearchArg = null);
    
    public record Filter(bool IsIsbn, bool IsTitle);
    
    public record SearchEventArgs(string Search, Filter Filer);
}