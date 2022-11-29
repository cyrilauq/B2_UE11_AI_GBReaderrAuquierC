using System;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Presentation;
using SearchOption = GBReaderAuquierC.Presentation.SearchOption;

namespace GBReaderAuquierC.Avalonia.Views
{
    public interface IHomeView
    {
        event EventHandler<DescriptionEventArgs> DisplayDetailsRequested;
        event EventHandler ReadBookRequested;
        event EventHandler<SearchEventArgs> SearchBookRequested;
        event EventHandler<ChangePageEventArgs> ChangePageRequested;

        void DisplayDetailsFor(BookExtendedItem item);

        void DisplayBook(List<Book> books);
    }

    public record ChangePageEventArgs(int Move);

    public record SearchEventArgs(string Search, SearchOption Option);
}