using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GBReaderAuquierC.Avalonia.Pages;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Presenter.ViewModel;

namespace GBReaderAuquierC.Avalonia;

public partial class ReadBookView : UserControl, IAskToDisplayMessage, IReadView
{
    private string _bookTitle;
    private PageViewModel _currentPage;
    private ReadingState _state = ReadingState.Continue;

    public event EventHandler<GotToPageEventArgs>? GoToPageRequested;

    public string BookTitle
    {
        set
        {
            _bookTitle = value;
            Refresh();
        }
    }

    public PageViewModel CurrentPage 
    {
        set
        {
            _currentPage = value ?? throw new ArgumentException("La page courante ne peut pas être nulle.");
            Refresh();
        }
    }

    public ReadingState ReadingState
    {
        set
        {
            _state = value;
            Refresh();
        }
    }

    public ReadBookView()
    {
        InitializeComponent();
    }

    public void AddListener(IDisplayMessages listener)
    {
        //throw new System.NotImplementedException();
    }

    public void OnEnter(string fromView)
    {
        
    }

    public void SetTitle(string title)
    {
        Title.Text = title;
    }

    public void SetCurrentPage(int nPage, string content)
    {
        NPage.Text = $"Page n° {nPage}:";
        Content.Text = content;
    }

    private void Refresh()
    {
        IList<PageChoiceView> items = new List<PageChoiceView>();
        Title.Text = _bookTitle;
        if (_currentPage != null)
        {
            NPage.Text = $"Page n° {_currentPage.Num}:";
            Content.Text = _currentPage.Text;
            foreach (var c in _currentPage.Choices)
            {
                var item = new PageChoiceView();
                item.Choice = c;
                items.Add(item);
            }
        }

        if (_currentPage == null || _currentPage.Choices.Count == 0)
        {
            Choices.IsVisible = false;
        }
        else
        {
            Choices.IsVisible = true;
            Choices.Items = items;
            Choices.SelectedIndex = 0;
        }

        HideButtons();
        if (_state == ReadingState.Continue)
        {
            ContinueTo.IsVisible = true;
        }
        else if (_state == ReadingState.Restart)
        {
            Restart.IsVisible = true;
        }
        else
        {
            Message.IsVisible = true;
            Message.Text = "Cette page est la seule page du livre.";
        }
    }

    private void HideButtons()
    {
        ContinueTo.IsVisible = false;
        Restart.IsVisible = false;
        Message.IsVisible = false;
    }

    private void OnValidClicked(object? sender, RoutedEventArgs e)
    {
        // TODO : Voir avec le prof si je peux laisser à la vue la gestion d'afficher bouton valider/recommencer ou alors le fait de ne pas mettre de bouton si il n'y as pas de page.
        // TODO : Voir si je peux pas utiliser le combobox avec un bouton valider plutôt que de mettre plusieurs boutons
        PageChoiceView sld = Choices.SelectedItem as PageChoiceView;
        GoToPageRequested?.Invoke(this, new GotToPageEventArgs(sld.NumTarget, sld.ContentTarget));
    }

    private void OnRestartClicked(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}