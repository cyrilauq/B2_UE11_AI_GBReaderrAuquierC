using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Repository;
using GBReaderAuquierC.Presenter;
using Newtonsoft.Json;

namespace GBReaderAuquierC.Avalonia;

public partial class HomeView : UserControl, View
{
    private readonly int MAX_BOOK_PAGE = 4;

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

    private Session _session;
    
    private Action<string> _router;
    
    public Action<string> Router { set => _router = value; }

    private TextBox _prenom;
    private TextBox _nom;
    private Button _connexionBtn;
    private StackPanel _booksPnl;
    private TextBlock _currentPageBlock;
    private TextBox _searchBox;
    private TextBlock _errorMsg;
    
    private readonly List<Book> _allBooks;

    public HomeView()
    {
        InitializeComponent();
        InitComponent();
        try
        {
            _allBooks = new List<Book>(GetBooks());
            RefreshBookPanel();
        }
        catch (Exception e)
        {
            _errorMsg.Text = "Error: " + e.Message;
            _errorMsg.IsVisible = true;
            /*var th = new Thread(this);
            th.Start();
            Thread.Sleep(5000);
            Environment.Exit(0);*/
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private List<Book> GetBooks()
    {
        List<BookDTO> temp = new JsonRepository().getData(
            Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "/ue36",
            "e200106.json");
        var result = new List<Book>();
        temp.ForEach(b => result.Add(b.toBook()));
        return result;
    }

    private void RefreshBookPanel()
    {
        _booksPnl.Children.Clear();

        for (int i = IndexBegin; i < IndexEnd && i < _allBooks.Count; i++)
        {
            var b = _allBooks[i];
            var temp = new DescriptionBookView();
            temp.SetBookInfo(title: b.Title, author: b.Author, isbn: b.ISBN, resume: b.Resume, imagePath: b.Image);
            _booksPnl.Children.Add(temp);
        }
        _currentPageBlock.Text = "" + NbCurrentPage;
    }

    private void InitComponent()
    {
        _prenom = this.FindControl<TextBox>("Prenom");
        _nom = this.FindControl<TextBox>("Nom");
        _connexionBtn = this.FindControl<Button>("Connexion");
        _booksPnl = this.FindControl<StackPanel>("Books");
        _currentPageBlock = this.FindControl<TextBlock>("CurrentPage");
        _errorMsg = this.FindControl<TextBlock>("ErrorMsg");
        _searchBox = this.FindControl<TextBox>("Search");
        _currentPage = 1;
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
        List<Book> search = new();
        string q = _searchBox.Text;
        if (q.Length == 0)
        {
            RefreshBookPanel();
        }
        else
        {
            _allBooks.ForEach(b =>
            {
                if (b.Title.ToLower().Contains(q) || b.ISBN.Contains(q))
                {
                    search.Add(b);
                }
            });
            _booksPnl.Children.Clear();
        
            search.ForEach(b => 
            {
                var temp = new DescriptionBookView();
                temp.SetBookInfo(title: b.Title, author: b.Author, isbn: b.ISBN, resume: b.Resume, imagePath: b.Image);
                _booksPnl.Children.Add(temp);
            });
        }
    }

    private void On_EnterDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            On_SearchedClicked(sender, e);
        }
    }
}