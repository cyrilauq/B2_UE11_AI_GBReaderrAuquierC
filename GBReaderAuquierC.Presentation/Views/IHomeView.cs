using System;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Presentation;
using SearchOption = GBReaderAuquierC.Presentation.SearchOption;

namespace GBReaderAuquierC.Avalonia.Views
{
    public interface IHomeView
    {
        // TODO : Changer les Display par des attributs dans les vue.
        event EventHandler<DescriptionEventArgs> DisplayDetailsRequested;
        event EventHandler ReadBookRequested;
        event EventHandler<SearchEventArgs> SearchBookRequested;
        event EventHandler<ChangePageEventArgs> ChangePageRequested;

        void DisplayDetailsFor(BookExtendedItem item);

        void DisplayBook(IList<Book> books);

        void DisplayMessage(string message);
    }

    public record ChangePageEventArgs(int Move);

    public record SearchEventArgs(string Search, SearchOption Option);
}