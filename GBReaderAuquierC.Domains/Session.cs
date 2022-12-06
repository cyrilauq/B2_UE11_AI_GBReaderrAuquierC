using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GBReaderAuquierC.Domains;

public class Session : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _currentBook;
    private Book _book;
    private Page _page;
    private IList<int> _history = new List<int>();

    public Book Book
    {
        set
        {
            if (value != null)
            {
                _book = value;
                NotifyPropertyChanged(nameof(Book));
                Page = _history.Count == 0 ? _book.First : _book[_history.Last() - 1];
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
                _history.Add(_book.GetNPageFor(_page));
                NotifyPropertyChanged(nameof(Page));
            }
        }
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}