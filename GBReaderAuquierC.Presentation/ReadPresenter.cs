using System.ComponentModel;
using GBReaderAuquierC.Avalonia;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Presenter
{
    public class ReadPresenter
    {
        private IReadView _view;
        private IDataRepository _repo;
        private Session _session;
        private IBrowseViews _router;
        private IDisplayMessages _notificationManager;
        
        public ReadPresenter(IReadView view, IDisplayMessages notificationManager, IBrowseViews router, Session session,
            IDataRepository repo)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _router = router ?? throw new ArgumentNullException(nameof(router));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));

            _session.PropertyChanged += OnSessionPropertyChanged;
        }
        
        private void OnSessionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_session.Page))
            {
                if (_session.Page != null)
                {
                    _view.SetCurrentPage(0, _session.Page.Content);
                }
            }
            if(e.PropertyName == nameof(_session.Book))
            {
                if (_session.Book != null)
                {
                    _view.SetTitle(_session.Book.Title);
                }
            }
        }
    }
}