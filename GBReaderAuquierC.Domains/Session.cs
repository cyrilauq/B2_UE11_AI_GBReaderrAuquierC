using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GBReaderAuquierC.Domains;

public class Session : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string? _currentBook;
    private Book? _book;

    public Book Book
    {
        set
        {
            _book = value;
            NotifyPropertyChanged(nameof(Book));
        }
        get => _book;
    }

    public string CurrentBook
    {
        set
        {
            _currentBook = value;
            NotifyPropertyChanged(nameof(CurrentBook));
        }
        get => _currentBook;
    }

    private void NotifyPropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}