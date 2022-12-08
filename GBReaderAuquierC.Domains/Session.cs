﻿using System.ComponentModel;

namespace GBReaderAuquierC.Domains;

public class Session : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _currentBook;
    private Book _book;
    private Page _page;
    private Dictionary<string, IList<int>> _history = new ();

    public Dictionary<string, IList<int>> History { get => new(_history);
        set
        {
            if (_history != null)
            {
                throw new ArgumentException("The session cannot be initialized twice.");
            }
            _history = value;
        }
    }

    public Book Book
    {
        set
        {
            if (value != null)
            {
                _book = value;
                NotifyPropertyChanged(nameof(Book));
                if (!_history.ContainsKey(_book.ISBN))
                {
                    _history[_book.ISBN] = new List<int>();
                }
                Page = _history.Count == 0 || _history[_book.ISBN].Count == 0 ? _book.First : _book[_history[_book.ISBN].Last() - 1];
            }
        }
        get => _book;
    }

    public Page Page
    {
        get => _page;
        set
        {
            if (value != null)
            {
                _page = value;
                _history[_book.ISBN].Add(_book.GetNPageFor(_page));
                NotifyPropertyChanged(nameof(Page));
            }
        }
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}