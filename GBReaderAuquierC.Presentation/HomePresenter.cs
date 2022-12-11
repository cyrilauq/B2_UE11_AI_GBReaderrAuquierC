using System.ComponentModel;
using GBReaderAuquierC.Avalonia;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Infrastructures.Exceptions;
using GBReaderAuquierC.Presentation.Views;
using GBReaderAuquierC.Presenter.Views;
using GBReaderAuquierC.Repositories;
using SearchOption = GBReaderAuquierC.Infrastructures.SearchOption;

namespace GBReaderAuquierC.Presentation;

public class HomePresenter
{
    private IHomeView _view;
    private IDataRepository _repo;
    private Session _session;
    private IBrowseViews _router;
    private IDisplayMessages _notificationManager;

    private int _currentPage = 0;

    private const int MAX_BOOK_PAGE = 8;

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
            var books = new List<Book>(_repo.GetBooks(_currentPage * MAX_BOOK_PAGE, MAX_BOOK_PAGE));
            if (books.Count == 0)
            {
                _view.DisplayMessage("Aucun livre n'a été trouvé.");
            } 
            else
            {
                Refresh();
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
        var books = new List<Book>(_repo.GetBooks((_currentPage + e.Move) * MAX_BOOK_PAGE, MAX_BOOK_PAGE));
        if (_currentPage + e.Move > -1 && books.Count > 0)
        {
            _currentPage += +e.Move;
            Refresh(books);
        }
    }

    private void Refresh(IEnumerable<Book> toDisplay = null)
    {
        IList<Book> books = toDisplay == null ? new List<Book>(_repo.GetBooks(_currentPage * MAX_BOOK_PAGE, MAX_BOOK_PAGE)) : new List<Book>(toDisplay);
        _session.CurrentBook = books.First();
        _view.Books = GetBookItems(books);
        _view.BookDetails = GetExtendedItem(books.First());
        _view.ActualPage = _currentPage + 1;
    }

    private IEnumerable<BookItem> GetBookItems(IEnumerable<Book> books)
    {
        IList<BookItem> result = new List<BookItem>();
        foreach (Book b in books)
        {
            result.Add(new BookItem(b.Title, b.Author, b.ISBN, b.Image));
        }
        return result;
    }

    private void DisplayDetails(object? sender, DescriptionEventArgs e)
    {
        _session.CurrentBook = _repo.Search(e.Isbn);
    }

    private void OnSearchRequested(object? sender, SearchEventArgs e)
    {
        IList<Book> result = new List<Book>(_repo.SearchBooks(e.Search,
                e.Filer.IsIsbn ? SearchOption.FilterIsbn :
                e.Filer.IsTitle ? SearchOption.FilterTitle :
                SearchOption.FilterBoth,
                new RangeArg(_currentPage, MAX_BOOK_PAGE)
            )
        );

        if (result.Count == 0)
        {
            _notificationManager.DisplayNotification(new NotifInfo("Recherche",
                "Aucun livre correspondant n'a été trouvé.", NotifSeverity.Error));
        } 
        else
        {
            _view.Books = GetBookItems(result);
            _view.BookDetails = GetExtendedItem(result.First());
        }
    }

    private void OnReadeBookClicked(object? sender, EventArgs eventArgs)
    {
        _session.Book = _repo.LoadBook(_session.CurrentBook.ISBN);
        _router.GoTo("ReadBookView");
    }

    private void OnSessionPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_session.CurrentBook))
        {
            _view.BookDetails = GetExtendedItem(_session.CurrentBook);
        }
    }

    private BookExtendedItem GetExtendedItem(Book book)
    {
        return new BookExtendedItem(
            book.Title,
            book.Author,
            book.ISBN,
            book.Image,
            book.Resume
        );
    }

    public void OnSessionSaved(object? sender, CancelEventArgs e)
    {
        try
        {
            _repo.SaveSession(_session);
        }
        catch (Exception)
        {
            _router.GoTo("HomeView");
        }
    }
}