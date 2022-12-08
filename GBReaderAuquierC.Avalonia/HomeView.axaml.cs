using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Interactivity;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Repositories;
using GBReaderAuquierC.Presentation;
using SearchOption = GBReaderAuquierC.Presentation.SearchOption;

namespace GBReaderAuquierC.Avalonia;

public partial class HomeView : UserControl, IView, IAskToDisplayMessage, IHomeView
{
    public event EventHandler<DescriptionEventArgs>? DisplayDetailsRequested;
    public event EventHandler ReadBookRequested;
    public event EventHandler<SearchEventArgs>? SearchBookRequested;
    public event EventHandler<ChangePageEventArgs> ChangePageRequested;
    
    private readonly int MAX_BOOK_PAGE = 8;
    private int test = 8;

    private int _currentPage = 1;

    private HashSet<IDisplayMessages> _listeners = new();

    private int NCurrentPage
    {
        set
        {
            if (value > 0 && (value - 1) * MAX_BOOK_PAGE < _allBooks.Count)
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
    
    private Session _session;

    public Session Session
    {
        set => _session = value;
        get => _session;
    }

    public HomeView()
    {
        InitializeComponent();
    }
    
    public void On_DescriptionClicked(object? sender, DescriptionEventArgs args)
        => DisplayDetailsRequested?.Invoke(this, args);

    public void On_ShowBookClicked(object? sender, DescriptionEventArgs args) 
        => ReadBookRequested?.Invoke(this, EventArgs.Empty);

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

    public void DisplayBook(IList<Book> books)
    {
        for (int i = IndexBegin; i < IndexEnd && i < books.Count; i++)
        {
            var b = books[i];
            if (i == 0)
            {
                //DiplayBook(b);
            }
            var temp = new DescriptionBookView();
            temp.SetBookInfo(new BookItem(b.Title, b.Author, b.ISBN, b.Image));
            temp.Display += On_DescriptionClicked;
            Books.Children.Add(temp);
        }
        CurrentPage.Text = "" + NCurrentPage;
    }

    private void DiplayBook(Book book)
    {
        if(book == null) { return; }
        var resumeBlock = this.FindControl<WrapPanel>("Details");
        resumeBlock.Children.Clear();
        var descr = new ExtendedDescriptionBookView();
        descr.Show += On_ShowBookClicked;
        descr.SetInfos(new BookExtendedItem(
            book.Title,
            book.Author,
            book.ISBN,
            book.Image,
            book.Resume));
        resumeBlock.Children.Add(descr);
    }

    public void DisplayMessage(string message)
        => Message.Text = message;

    public void DisplayDetailsFor(BookExtendedItem item)
    {
        var details = new ExtendedDescriptionBookView();
        details.SetInfos(item);
        details.Show += On_ShowBookClicked;
        Details.Children.Clear();
        Details.Children.Add(details);
    }

    public void GoTo(string toView) 
        => _router.Invoke(toView);

    private void On_PreviousClicked(object? sender, RoutedEventArgs e)
    {
        ChangePageRequested?.Invoke(this, new ChangePageEventArgs(-1));
    }

    private void On_NextClicked(object? sender, RoutedEventArgs e)
    {
        ChangePageRequested?.Invoke(this, new ChangePageEventArgs(1));
    }

    private void On_SearchedClicked(object? sender, RoutedEventArgs e)
    {
        string q = Search.Text;
        if (q == null || q.Trim().Length == 0)
        {
            _search = false;
            //RefreshBookPanel();
        }
        else
        {
            _search = true;
            _currentPage = 1;
            _searchBooks.Clear();
            FindBooksFor(q).ForEach(b => _searchBooks.Add(b));
            //RefreshBookPanel();
        }
    }

    private List<Book> FindBooksFor(string search)
    {
        var searchOption = SearchOption.NoFilter;
        if (FilterTitle.IsSelected)
        {
            searchOption = SearchOption.FilterTitle;
        } 
        else if (FilterISBN.IsSelected)
        {
            searchOption = SearchOption.FilterIsbn;
        }
        
        SearchBookRequested?.Invoke(this, new SearchEventArgs(search.ToLower().Replace("-", ""), searchOption));
        
        return new();
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

    public void OnEnter(string fromView)
    {
        
    }
}