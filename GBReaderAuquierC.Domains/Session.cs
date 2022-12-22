using System.ComponentModel;

namespace GBReaderAuquierC.Domains;

public class Session : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private Book _currentBook;
    private Book _book;
    private Page _page;
    private Dictionary<string, BookSave> _history = new ();

    public Book CurrentBook
    {
        get => _currentBook;
        set
        {
            if (value != null)
            {
                _currentBook = value;
                NotifyPropertyChanged(nameof(CurrentBook));
            }
        }
    }

    public Dictionary<string, BookSave> History
    {
        get => _history;
        set
        {
            if (_history != null && _history.Count > 0)
            {
                throw new ArgumentException("The session cannot be initialized twice.");
            }
            _history = value;
            if (value.Count > 0)
            {
                NotifyPropertyChanged(nameof(Book));
            }
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
                if (!_history.ContainsKey(_book[BookAttribute.Isbn]))
                {
                    _history[_book[BookAttribute.Isbn]] = new BookSave();
                }
                Page = _history.Count == 0 || _history[_book[BookAttribute.Isbn]].Count == 0 ? _book.First : _book[_history[_book[BookAttribute.Isbn]].Last - 1];
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
                if (_page.IsTerminal)
                {
                    _history.Remove(_book[BookAttribute.Isbn]);
                }
                else
                {
                    _history[_book[BookAttribute.Isbn]].Add(_book.GetNPageFor(_page));
                }
                NotifyPropertyChanged(nameof(Page));
            }
        }
    }

    private void NotifyPropertyChanged(string propertyName) 
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}