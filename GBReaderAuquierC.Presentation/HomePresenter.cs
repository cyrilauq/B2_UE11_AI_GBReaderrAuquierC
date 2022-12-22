using System.ComponentModel;
using GBReaderAuquierC.Avalonia;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Infrastructures.Exceptions;
using GBReaderAuquierC.Presentation.Views;
using GBReaderAuquierC.Presenter.Views;
using SearchOption = GBReaderAuquierC.Infrastructures.SearchOption;

namespace GBReaderAuquierC.Presentation;

public class HomePresenter
{
    private IHomeView _view;
    private IDataRepository _repo;
    private ISessionRepository _session;
    private IBrowseViews _router;
    private IDisplayMessages _notificationManager;

    private const int MAX_BOOK_PAGE = 8;
    
    private int CurrentPage { get => _view.ActualPage - 1; }

    public HomePresenter(IHomeView view, IDisplayMessages notificationManager, IBrowseViews router, ISessionRepository session, IDataRepository repo)
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
        _view.ViewStatRequested += OnViewStatRequested;
        
        try
        {
            var books = new List<Book>(_repo.GetBooks(0, MAX_BOOK_PAGE));
            if (books.Count == 0)
            {
                _view.DisplayMessage("Aucun livre n'a été trouvé.");
            } 
            else
            {
                Refresh();
            }
            _session.LoadSession();
        }
        catch (DataManipulationException e)
        {
            _view.DisplayMessage(e.Message);
        }
    }

    private void ChangePageRequested(object? sender, ChangePageEventArgs e)
    {
        IList<Book> books = new List<Book>();
        if (e.SearchArg == null)
        {
            books = new List<Book>(_repo.GetBooks((CurrentPage + e.Move) * MAX_BOOK_PAGE, MAX_BOOK_PAGE));
        }
        else
        {
            books = new List<Book>(_repo.SearchBooks(e.SearchArg.Search, 
                e.SearchArg.Filer.IsIsbn ? SearchOption.FilterIsbn :
                e.SearchArg.Filer.IsTitle ? SearchOption.FilterTitle :
                SearchOption.FilterBoth, new RangeArg((CurrentPage + e.Move) * MAX_BOOK_PAGE, MAX_BOOK_PAGE)));            
        }
        if (CurrentPage + e.Move > -1 && books.Count > 0)
        {
            Refresh(books, CurrentPage + e.Move);
        }
    }

    private void Refresh(IEnumerable<Book> toDisplay = null, int numPage = 1)
    {
        IList<Book> books = toDisplay == null ? new List<Book>(_repo.GetBooks(CurrentPage * MAX_BOOK_PAGE, MAX_BOOK_PAGE)) : new List<Book>(toDisplay);
        _session.CurrentBook = books.First();
        _view.Books = GetBookItems(books);
        _view.BookDetails = GetExtendedItem(books.First());
        _view.ActualPage = numPage;
    }

    private IEnumerable<BookItem> GetBookItems(IEnumerable<Book> books)
    {
        IList<BookItem> result = new List<BookItem>();
        foreach (Book b in books)
        {
            result.Add(new BookItem(
                b[BookAttribute.Title], 
                b[BookAttribute.Author], 
                b[BookAttribute.Isbn], 
                b[BookAttribute.Image]));
        }
        return result;
    }

    private void DisplayDetails(object? sender, DescriptionEventArgs e)
    {
        _session.CurrentBook = _repo.Search(e.Isbn);
    }

    private void OnSearchRequested(object? sender, SearchEventArgs e)
    {
        IList<Book> result =  new List<Book>(
            (e.Search is null || e.Search.Trim().Length == 0) ? _repo.GetBooks(CurrentPage * MAX_BOOK_PAGE, MAX_BOOK_PAGE) : 
            _repo.SearchBooks(e.Search,
                e.Filer.IsIsbn ? SearchOption.FilterIsbn :
                e.Filer.IsTitle ? SearchOption.FilterTitle :
                SearchOption.FilterBoth,
                new RangeArg(CurrentPage, MAX_BOOK_PAGE)
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
        _session.ReadingBook = _repo.LoadBook(_session.CurrentBook[BookAttribute.Isbn]);
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
            book[BookAttribute.Title],
            book[BookAttribute.Author],
            book[BookAttribute.Isbn],
            book[BookAttribute.Image],
            book[BookAttribute.Resume]
        );
    }

    private void OnViewStatRequested(object? sender, EventArgs e)
    {
        _router.GoTo("StatisticsView");
    }

    public void OnSessionSaved(object? sender, CancelEventArgs e)
    {
        try
        {
            _session.SaveSession();
        }
        catch (Exception)
        {
            _router.GoTo("HomeView");
        }
    }
}