using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GBReaderAuquierC.Domains;

public class Session : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _currentBook;
    private Book _book;
    private Page _page;

    public Book Book
    {
        set
        {
            if (value != null)
            {
                _book = value;
                NotifyPropertyChanged(nameof(Book));
                Page = _book.First;
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
                NotifyPropertyChanged(nameof(Page));
            }
        }
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}