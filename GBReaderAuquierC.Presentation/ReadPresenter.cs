using System.ComponentModel;
using GBReaderAuquierC.Avalonia;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Presenter.ViewModel;
using GBReaderAuquierC.Presenter.Views;

namespace GBReaderAuquierC.Presenter
{
    public class ReadPresenter
    {
        private IReadView _view;
        private ISessionRepository _session;
        private IBrowseViews _router;
        private IDisplayMessages _notificationManager;
        
        public ReadPresenter(IReadView view, IDisplayMessages notificationManager, IBrowseViews router, ISessionRepository session)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _router = router ?? throw new ArgumentNullException(nameof(router));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));

            _session.PropertyChanged += OnSessionPropertyChanged;
            _view.GoToPageRequested += GoToPageRequested;
            _view.RestartRequested += RestartRequested;
            _view.HomeRequested += HomeRequested;
        }

        private void OnSessionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_session.ReadingPage))
            {
                if (_session.ReadingPage != null)
                {
                    var currentPage = _session.ReadingPage;
                    var currentBook = _session.ReadingBook;
                    _view.CurrentPage = new PageViewModel(
                        currentPage.Content,
                        currentBook.GetNPageFor(currentPage),
                        GetChoiceViewModels(currentPage, _session.ReadingBook)
                    );
                    _view.ReadingState = ReadingState.Continue;
                    if (currentBook.CountPage > 1 && !currentPage.HasChoices)
                    {
                        _view.ReadingState = ReadingState.Restart;
                    }
                    else if (currentBook.CountPage == 1 || !currentPage.HasChoices)
                    {
                        _view.ReadingState = ReadingState.Nothing;
                    }
                }
            }
            if(e.PropertyName == nameof(_session.ReadingBook))
            {
                if (_session.ReadingBook != null)
                {
                    _view.BookTitle = _session.ReadingBook[BookAttribute.Title];
                }
            }
        }
        
        private IList<ChoiceViewModel> GetChoiceViewModels(Page page, Book book)
        {
            IList<ChoiceViewModel> result = new List<ChoiceViewModel>();
            foreach (var c in page.Choices.Keys)
            {
                result.Add(new ChoiceViewModel(
                    book.GetNPageFor(page.Choices[c]),
                    c
                ));
            }
            return result;
        }

        private void GoToPageRequested(object? sender, GotToPageEventArgs e)
        {
            _session.ReadingPage = _session.ReadingPage.GetPageFor(e.Content);
        }

        private void RestartRequested(object? sender, EventArgs e)
        {
            _session.ReadingBook = _session.ReadingBook;
        }

        private void HomeRequested(object? sender, EventArgs e)
        {
            _router.GoTo("HomeView");
        }
    }
}