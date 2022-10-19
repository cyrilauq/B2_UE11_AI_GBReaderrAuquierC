using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Repository;
using GBReaderAuquierC.Presenter;

namespace GBReaderAuquierC.Avalonia;

public partial class HomeView : UserControl, IView
{
    private readonly int MAX_BOOK_PAGE = 8;

    private int _currentPage = 1;

    private int NbCurrentPage
    {
        set
        {
            if (value > 0 && value * MAX_BOOK_PAGE <= _allBooks.Count)
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

    private readonly Session _session;
    
    private Action<string> _router;
    
    public Action<string> Router { set => _router = value; }
    
    private readonly List<Book> _allBooks;
    private readonly List<Book> _searchBooks = new();

    private IDataRepository _repo;

    private bool _search = false;

    public HomeView()
    {
        InitializeComponent();
        try
        {
            _repo = new JsonRepository(Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "/ue36",
                "e200106.json");
            _allBooks = new List<Book>(GetBooks());
            RefreshBookPanel();
        }
        catch (Exception e)
        {
            ErrorMsg.Text = "Error: " + e.Message;
            ErrorMsg.IsVisible = true;
        }
        // TODO : uatiliser un NotificationManager pour afficher les erreurs
    }

    private List<Book> GetBooks()
    {
        try
        {
            List<BookDTO> temp = _repo.GetData();
            var result = new List<Book>();
            // TODO : ajouter seulement les livres ayant un ISBN valide et ayant un titre, un auteur et un résumé.
            temp.ForEach(b =>
            {
                var v = b.ToBook();
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
        catch (FileNotFoundException e)
        {
            SetErrorMsg("Le fichier JSON n'a pas été trouvé...");
            return new List<Book>();
        }
        catch (DirectoryNotFoundException e)
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
            var temp = new DescriptionBookView();
            temp.SetBookInfo(title: b.Title, author: b.Author, isbn: b.ISBN, resume: b.Resume, imagePath: b.Image);
            temp.PointerPressed += DisplayDetails;
            Books.Children.Add(temp);
        }
        CurrentPage.Text = "" + NbCurrentPage;
    }

    private void DisplayDetails(object? sender, PointerEventArgs e)
    {
        var view = (DescriptionBookView)sender;
        var result = _repo.Search(view.Isbn.Text);
        var resumeBlock = this.FindControl<WrapPanel>("Details");
        resumeBlock.Children.Clear();
        var descr = new ExtendedDescriptionBookView();
        descr.SetInfos(new BookExtendedItem(
            result.Title,
            result.Author,
            result.ISBN,
            result.Image,
            result.Resume));
        resumeBlock.Children.Add(descr);
    }

    public void GoTo(string toView) 
        => _router.Invoke(toView);

    private void On_PreviousClicked(object? sender, RoutedEventArgs e)
    {
        NbCurrentPage--;
        RefreshBookPanel();
    }

    private void On_NextClicked(object? sender, RoutedEventArgs e)
    {
        NbCurrentPage++;
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

    private List<Book> FindBooksFor(string q)
    {
        var result = _allBooks.Where(b => b.Title.ToLower().Contains(q) || b.ISBN.Contains(q)).ToList();

        /*if (result is null)
        {
            throw new JsonRepository.NoBooksFindException("No book find for the search: " + q);
        }*/
        
        return result;
    }

    private void On_EnterDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            On_SearchedClicked(sender, e);
        }
    }
}