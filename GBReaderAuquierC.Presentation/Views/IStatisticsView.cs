using GBReaderAuquierC.Presenter.ViewModel;

namespace GBReaderAuquierC.Presentation.Views
{
    public interface IStatisticsView
    {
        event EventHandler GoHomeRequested;
        IEnumerable<StatViewModel> Books { set; }
        int ReadingBookCount { set; }
    }
}