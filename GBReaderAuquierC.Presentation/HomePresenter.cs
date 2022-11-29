using System.ComponentModel;
using GBReaderAuquierC.Avalonia;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Presentation;

public class HomePresenter
{
    private IHomeView _view;
    private IDataRepository _repo;
    private Session _session;
    private IBrowseViews _router;

    public HomePresenter(IHomeView view, IBrowseViews router, Session session, IDataRepository repo)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _session = session ?? throw new ArgumentNullException(nameof(session));
        
        _session.PropertyChanged += OnSessionPropertyChanged;
        _view.DisplayDetailsRequested += DisplayDetails;
        _view.ReadBookRequested += OnReadeBookClicked;
        _view.SearchBookRequested += OnSearchRequested;

        _repo = new JsonRepository(Path.Join(Environment.GetEnvironmentVariable("USERPROFILE"), "ue36"),
            "e200106.json");
        try
        {
            _session.Book = GetBooks().First();
        }
        catch (Exception e)
        {
            
        }
        _view.DisplayBook(GetBooks());
    }

    private List<Book> GetBooks()
    {
        try
        {
            List<BookDTO> temp = _repo.GetData();
            var result = new List<Book>();
            temp.ForEach(b =>
            {
                var v = Mapper.ConvertToBook(b);
                if (v)
                {
                    result.Add(v);
                }
            });
            if (result.Count == 0)
            {
                
            }
            return result;
        }
        catch (FileNotFoundException e)
        {
            
            return new List<Book>();
        }
        catch (DirectoryNotFoundException e)
        {
            
            return new List<Book>();
        }
    }

    private void DisplayDetails(object? sender, DescriptionEventArgs e)
    {
        _session.Book = _repo.Search(e.Isbn);
    }

    private void OnCurrentBookChanged(object? sender, PropertyChangedEventArgs e)
    {
        
    }

    private void OnSearchRequested(object? sender, SearchEventArgs e)
    {
        List<Book> result;
        if (e.Option.Equals(SearchOption.FilterIsbn))
        {
            result = GetBooks().Where(b => b.ISBN.ToLower().Replace("-", "").Contains(e.Search)).ToList();
        } 
        else if (e.Option.Equals(SearchOption.FilterTitle))
        {
            result = GetBooks().Where(b => b.Title.Replace("-", "").Contains(e.Search)).ToList();
        }
        else
        {
            result = GetBooks().Where(b => b.Title.Replace("-", "").ToLower().Contains(e.Search) || b.ISBN.Replace("-", "").Contains(e.Search)).ToList();
        }
        _view.DisplayBook(result);
    }

    private void OnReadeBookClicked(object? sender, EventArgs eventArgs) => _router.GoTo("ReadBookView");

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