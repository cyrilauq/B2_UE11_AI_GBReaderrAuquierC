using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Interactivity;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Repositories;
using GBReaderAuquierC.Presentation;

namespace GBReaderAuquierC.Avalonia;

public partial class HomeView : UserControl, IView, IAskToDisplayMessage
{
    private readonly int MAX_BOOK_PAGE = 8;

    private int _currentPage = 1;

    private HashSet<IDisplayMessages> _listeners = new();

    private int NCurrentPage
    {
        set
        {
            if (value > 0 && (value - 1) * MAX_BOOK_PAGE <= _allBooks.Count)
            {
                _currentPage = value;
            }
        }
        get => _currentPage;
    }

    private int IndexBegin
    {
        get => (_currentPage - 1) * MAX_BOOK_PAGE;
    }

    private int IndexEnd
    {
        get => _currentPage * MAX_BOOK_PAGE;
    }
    
    private Action<string> _router;
    
    public Action<string> Router { set => _router = value; }
    
    private readonly List<Book> _allBooks;
    private readonly List<Book> _searchBooks = new();

    private IDataRepository _repo;

    private bool _search = false;

    public HomeView()
    {
        InitializeComponent();
        _repo = new JsonRepository(Environment.GetEnvironmentVariable("USERPROFILE") + "/ue36",
            "e200106.json");
        try
        {
            _allBooks = new List<Book>(GetBooks());
            RefreshBookPanel();
        }
        catch (Exception e)
        {
            _allBooks = new List<Book>();
            ErrorMsg.Text = "Error: " + e.Message;
            ErrorMsg.IsVisible = true;
        }
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
                SetErrorMsg("Aucun livre n'a pu être trouvé...");
            }
            return result;
        }
        catch (FileNotFoundException)
        {
            SetErrorMsg("Le fichier JSON n'a pas été trouvé...");
            return new List<Book>();
        }
        catch (DirectoryNotFoundException)
        {
            SetErrorMsg("Le dossier n'a pas été trouvé...");
            return new List<Book>();
        }
    }

    private void SetErrorMsg(string msg)
    {
        ErrorMsg.Text = msg;
        ErrorMsg.IsVisible = true;
    }

    private void RefreshBookPanel()
    {
        Books.Children.Clear();

        DisplayBook(_search ? _searchBooks : _allBooks);
    }

    private void DisplayBook(List<Book> books)
    {
        for (int i = IndexBegin; i < IndexEnd && i < books.Count; i++)
        {
            var b = books[i];
            if (i == 0)
            {
                DiplayBook(b);
            }
            var temp = new DescriptionBookView();
            temp.SetBookInfo(title: b.Title, author: b.Author, isbn: b.ISBN, imagePath: b.Image);
            temp.PointerPressed += DisplayDetails;
            Books.Children.Add(temp);
        }
        CurrentPage.Text = "" + NCurrentPage;
    }

    private void DisplayDetails(object? sender, PointerEventArgs e)
    {
        var view = (DescriptionBookView)sender!;
        DiplayBook(_repo.Search(view.Isbn.Text));
    }

    private void DiplayBook(Book book)
    {
        if(book == null) { return; }
        var resumeBlock = this.FindControl<WrapPanel>("Details");
        resumeBlock.Children.Clear();
        var descr = new ExtendedDescriptionBookView();
        descr.SetInfos(new BookExtendedItem(
            book.Title,
            book.Author,
            book.ISBN,
            book.Image,
            book.Resume));
        resumeBlock.Children.Add(descr);
    }

    public void GoTo(string toView) 
        => _router.Invoke(toView);

    private void On_PreviousClicked(object? sender, RoutedEventArgs e)
    {
        NCurrentPage--;
        RefreshBookPanel();
    }

    private void On_NextClicked(object? sender, RoutedEventArgs e)
    {
        NCurrentPage++;
        RefreshBookPanel();
    }

    private void On_SearchedClicked(object? sender, RoutedEventArgs e)
    {
        string q = Search.Text;
        if (q == null || q.Trim().Length == 0)
        {
            _search = false;
            RefreshBookPanel();
        }
        else
        {
            _search = true;
            _currentPage = 1;
            _searchBooks.Clear();
            FindBooksFor(q).ForEach(b => _searchBooks.Add(b));
            RefreshBookPanel();
        }
    }

    private List<Book> FindBooksFor(string search)
    {
        search = search.ToLower().Replace("-", "");
        List<Book> result;
        if (FilterTitle.IsSelected)
        {
            result = _allBooks.Where(b => b.Title.ToLower().Replace("-", "").Contains(search)).ToList();
        } else if (FilterISBN.IsSelected)
        {
            result = _allBooks.Where(b => b.ISBN.Replace("-", "").Contains(search)).ToList();
        }
        else
        {
            result = _allBooks.Where(b => b.Title.Replace("-", "").ToLower().Contains(search) || b.ISBN.Replace("-", "").Contains(search)).ToList();
        }

        if (result.Count == 0)
        {
            NotifyListeners(new Notification("Recherche","Aucun livre correspondant n'a été trouvé.",NotificationType.Error));
        }
        
        return result;
    }

    private void On_EnterDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            On_SearchedClicked(sender, e);
        }
    }

    public void AddListener(IDisplayMessages listener)
    {
        _listeners.Add(listener);
    }

    private void NotifyListeners(Notification notif)
    {
        foreach (var l in _listeners)
        {
            l.DisplayNotification(notif);
        }
    }
}