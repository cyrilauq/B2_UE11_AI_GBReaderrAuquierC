using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Presenter.ViewModel;

namespace GBReaderAuquierC.Avalonia.Views
{
    public interface IReadView
    {
        event EventHandler<GotToPageEventArgs> GoToPageRequested; 

        string BookTitle { set; }
        PageViewModel CurrentPage { set; }
        ReadingState ReadingState { set; }
    
        void SetTitle(string title);

        void SetCurrentPage(int nPage, string content);
    }

    public record GotToPageEventArgs(int Num, string Content);
}