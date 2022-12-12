using System.ComponentModel;
using GBReaderAuquierC.Avalonia;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Presentation.Views;
using GBReaderAuquierC.Presenter.ViewModel;
using GBReaderAuquierC.Presenter.Views;

namespace GBReaderAuquierC.Presentation
{
    public class StatisticsPresenter
    {
        private IStatisticsView _view;
        private IBrowseViews _router;
        private IDisplayMessages _notification;
        private Session _session;

        public StatisticsPresenter(IStatisticsView view, IBrowseViews router, IDisplayMessages notification,
            Session sessions)
        {
            _view = view;
            _router = router;
            _notification = notification;
            _session = sessions;

            _session.PropertyChanged += NotifyPropertyChanged;
            _view.GoHomeRequested += OnGoHome;
            
            Refresh();
        }

        private void OnGoHome(object? sender, EventArgs e)
        {
            _router.GoTo("HomeView");
        }

        private void Refresh()
        {
            _view.ReadingBookCount = _session.History.Count;
            IList<StatViewModel> svms = new List<StatViewModel>();
            foreach (var he in _session.History)
            {
                svms.Add(new StatViewModel(
                        null,
                        he.Value.Begin.ToString("Le dd MMMM yyyy à hh:mm:ss"),
                        he.Value.Begin.ToString("Le dd month yyyy à hh:mm:ss"),
                        he.Key
                    )
                );
            }

            _view.Books = svms;
        }

        private void NotifyPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_session.Book))
            {
                Refresh();
            }
        }
    }
}