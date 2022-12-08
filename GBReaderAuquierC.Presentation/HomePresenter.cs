using System.ComponentModel;
using GBReaderAuquierC.Avalonia;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Infrastructures.Exceptions;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Presentation;

public class HomePresenter
{
    private IHomeView _view;
    private IDataRepository _repo;
    private Session _session;
    private IBrowseViews _router;
    private IDisplayMessages _notificationManager;
    private Book _currentBook;

    private int _currentPage = 0;
    
    private readonly int MAX_BOOK_PAGE = 8;

    public HomePresenter(IHomeView view, IDisplayMessages notificationManager, IBrowseViews router, Session session, IDataRepository repo)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
        
        _session.PropertyChanged += OnSessionPropertyChanged;
        _view.DisplayDetailsRequested += DisplayDetails;
        _view.ReadBookRequested += OnReadeBookClicked;
        _view.SearchBookRequested += OnSearchRequested;
        _view.ChangePageRequested += ChangePageRequested;
        
        try
        {
            var books = _repo.GetBooks();
            if (books.Count == 0)
            {
                _view.DisplayMessage("Aucun livre n'a été trouvé.");
            } 
            else
            {
                _currentBook = books.First();
                _view.DisplayBook(_repo.GetBooks());
            }
            _repo.LoadSession(_session);
        }
        catch (DataManipulationException e)
        {
            _view.DisplayMessage(e.Message);
        }
    }

    private void ChangePageRequested(object? sender, ChangePageEventArgs e)
    {
        if (_currentPage + e.Move > -1 && _currentPage + e.Move < (_repo.GetBooks().Count / MAX_BOOK_PAGE))
        {
            // _view.DisplayBook(_repo.GetBooks()[new Range(_currentPage, MAX_BOOK_PAGE)]);
        }
    }

    private void DisplayDetails(object? sender, DescriptionEventArgs e)
    {
        _currentBook = _repo.Search(e.Isbn);
        _view.DisplayDetailsFor(new BookExtendedItem(
            _currentBook.Title,
            _currentBook.Author,
            _currentBook.ISBN,
            _currentBook.Image,
            _currentBook.Resume));
    }

    private void OnSearchRequested(object? sender, SearchEventArgs e)
    {
        List<Book> result;
        if (e.Option.Equals(SearchOption.FilterIsbn))
        {
            result = _repo.GetBooks().Where(b => b.ISBN.ToLower().Replace("-", "").Contains(e.Search)).ToList();
        } 
        else if (e.Option.Equals(SearchOption.FilterTitle))
        {
            result = _repo.GetBooks().Where(b => b.Title.Replace("-", "").Contains(e.Search)).ToList();
        }
        else
        {
            result = _repo.GetBooks().Where(b => b.Title.Replace("-", "").ToLower().Contains(e.Search) || b.ISBN.Replace("-", "").Contains(e.Search)).ToList();
        }

        if (result.Count == 0)
        {
            _notificationManager.DisplayNotification(new NotifInfo("Recherche",
                "Aucun livre correspondant n'a été trouvé.", NotifSeverity.Error));
        } 
        else
        {
            _view.DisplayBook(result);
        }
    }

    private void OnReadeBookClicked(object? sender, EventArgs eventArgs)
    {
        _session.Book = _repo.LoadBook(_currentBook.ISBN);
        _router.GoTo("ReadBookView");
    }

    private void OnSessionPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_session.Book))
        {
            if (_session.Book != null)
            {
                _view.DisplayDetailsFor(new BookExtendedItem(
                    _session.Book.Title,
                    _session.Book.Author,
                    _session.Book.ISBN,
                    _session.Book.Image,
                    _session.Book.Resume)
                );
            }
        }
    }
}