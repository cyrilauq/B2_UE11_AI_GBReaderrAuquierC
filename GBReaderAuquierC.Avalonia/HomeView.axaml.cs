using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Presentation.Views;

namespace GBReaderAuquierC.Avalonia;

public partial class HomeView : UserControl, IHomeView
{
    public event EventHandler<DescriptionEventArgs>? DisplayDetailsRequested;
    public event EventHandler ReadBookRequested;
    public event EventHandler<SearchEventArgs>? SearchBookRequested;
    public event EventHandler ViewStatRequested;
    public event EventHandler<ChangePageEventArgs> ChangePageRequested;

    public int ActualPage
    {
        set => CurrentPage.Text = "" + value;
        get => Int32.Parse(CurrentPage.Text);
    }

    public IEnumerable<BookItem> Books
    {
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException("The given books shouldn't be null.");
            }
            DisplayDescriptions(value);
        }
    }

    private void DisplayDescriptions(IEnumerable<BookItem> value)
    {
        BooksPanel.Children.Clear();
        foreach (var bi in value)
        {
            var temp = new DescriptionBookView();
            temp.SetBookInfo(bi);
            temp.Display += OnDescriptionClicked;
            BooksPanel.Children.Add(temp);
        }
    }

    public BookExtendedItem BookDetails
    {
        set
        {
            if (value is null)
            {
                throw new ArgumentException("The given item shouldn't be null.");
            }
            DisplayDetailsFor(value);
        }
    }

    public HomeView()
    {
        InitializeComponent();
    }

    private void OnDescriptionClicked(object? sender, DescriptionEventArgs args)
        => DisplayDetailsRequested?.Invoke(this, args);

    private void OnShowBookClicked(object? sender, DescriptionEventArgs args) 
        => ReadBookRequested?.Invoke(this, EventArgs.Empty);

    private void SetErrorMsg(string msg)
    {
        ErrorMsg.Text = msg;
        ErrorMsg.IsVisible = true;
    }

    public void DisplayMessage(string message)
        => Message.Text = message;

    private void DisplayDetailsFor(BookExtendedItem item)
    {
        var details = new ExtendedDescriptionBookView();
        details.SetInfos(item);
        details.Show += OnShowBookClicked;
        Details.Children.Clear();
        Details.Children.Add(details);
    }

    private void On_PreviousClicked(object? sender, RoutedEventArgs e) 
        => ChangePageRequested?.Invoke(this, new ChangePageEventArgs(-1, GetSearchArgs()));

    private void On_NextClicked(object? sender, RoutedEventArgs e) 
        => ChangePageRequested?.Invoke(this, new ChangePageEventArgs(1, GetSearchArgs()));

    private void On_SearchedClicked(object? sender, RoutedEventArgs e)
    {
        SearchBookRequested?.Invoke(this, GetSearchArgs());
    }

    private SearchEventArgs GetSearchArgs() 
        => new (Search.Text?.ToLower().Replace("-", ""),
            new Filter(
                FilterISBN.IsSelected,
                FilterTitle.IsSelected
            )
        );

    private void On_EnterDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            On_SearchedClicked(sender, e);
        }
    }

    private void On_ViewStatClicked(object? sender, RoutedEventArgs e)
    {
        ViewStatRequested?.Invoke(this, EventArgs.Empty);
    }
}