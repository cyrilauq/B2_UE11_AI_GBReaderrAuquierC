﻿using System;
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
    private IList<PageChoiceView> _items = new List<PageChoiceView>();

    public IList<PageChoiceView> Items
    {
        get => _items;
    }

    public event EventHandler<GotToPageEventArgs>? GoToPageRequested;
    public event EventHandler RestartRequested;
    public event EventHandler HomeRequested;

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
        Title.Text = _bookTitle;
        if (_currentPage != null)
        {
            Choices.Children.Clear();
            NPage.Text = $"Page n° {_currentPage.Num}:";
            Content.Text = _currentPage.Text;
            foreach (var c in _currentPage.Choices)
            {
                var item = new PageChoiceView();
                item.Choice = c;
                item.Click += OnValidClicked;
                Choices.Children.Add(item);
            }
        }

        ChoicePnl.IsVisible = _currentPage != null && _currentPage.Choices.Count > 0;

        HideButtons();
        Restart.IsVisible = _state == ReadingState.Nothing || _state == ReadingState.Restart;
        if (_state == ReadingState.Nothing)
        {
            Message.IsVisible = true;
            Message.Text = "Aucune action ne peut être réalisée à partir de cette page...";
        }
    }

    private void HideButtons()
    {
        Restart.IsVisible = false;
        Message.IsVisible = false;
        Home.IsVisible = true;
    }

    private void OnValidClicked(object? sender, GotToPageEventArgs e)
    {
        // TODO : Voir avec le prof si je peux laisser à la vue la gestion d'afficher bouton valider/recommencer ou alors le fait de ne pas mettre de bouton si il n'y as pas de page.
        // TODO : Voir si je peux pas utiliser le combobox avec un bouton valider plutôt que de mettre plusieurs boutons
        GoToPageRequested?.Invoke(this, e);
    }

    private void OnRestartClicked(object? sender, RoutedEventArgs e)
    {
        RestartRequested?.Invoke(this, e);
    }

    private void OnHomeClicked(object? sender, RoutedEventArgs e)
    {
        HomeRequested?.Invoke(this, e);
    }
}