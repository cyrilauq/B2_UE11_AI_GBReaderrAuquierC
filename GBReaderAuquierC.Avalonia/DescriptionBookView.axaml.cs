using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace GBReaderAuquierC.Avalonia;

public partial class DescriptionBookView : UserControl
{
    public DescriptionBookView()
    {
        InitializeComponent();
    }
    
    public void SetBookInfo(string title, string isbn, string author, string imagePath)
    {
        Title.Text = title;
        Author.Text = author;
        Isbn.Text = isbn;
        if (imagePath != null && imagePath.Trim().Length > 0)
        {
            Image.Source = new Bitmap(imagePath);
        }
    }
}