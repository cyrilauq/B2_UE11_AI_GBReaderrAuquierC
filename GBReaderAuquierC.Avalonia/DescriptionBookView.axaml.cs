using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Presentation;

namespace GBReaderAuquierC.Avalonia;

public partial class DescriptionBookView : UserControl
{
    private string _isbn;

    public DescriptionBookView()
    {
        InitializeComponent();
        PointerPressed += OnDisplayDetailsRequested;
    }

    private void OnDisplayDetailsRequested(object? sender, RoutedEventArgs args)
    {
        Display?.Invoke(this, new DescriptionEventArgs(Isbn.Text));
    }

    public void SetBookInfo(BookItem book)
    {
        Title.Text = book.Title;
        Author.Text = book.Author;
        Isbn.Text = _isbn = book.Isbn;
        if (book.ImgPath != null && book.ImgPath.Trim().Length > 0)
        {
            Image.Source = new Bitmap(book.ImgPath);
        }
    }
    
    public event DescriptionEventHandler Display;
}