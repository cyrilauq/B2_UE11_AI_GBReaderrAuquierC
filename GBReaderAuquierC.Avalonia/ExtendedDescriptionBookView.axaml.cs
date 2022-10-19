using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using GBReaderAuquierC.Presenter;

namespace GBReaderAuquierC.Avalonia;

public partial class ExtendedDescriptionBookView : UserControl
{
    private TextBlock _title;
    private TextBlock _author;
    private TextBlock _isbn;
    private TextBlock _resume;

    public ExtendedDescriptionBookView()
    {
        InitializeComponent();
    }

    public void SetInfos(BookExtendedItem item)
    {
        Title.Text = item.Title;
        Resume.Text = item.Resume;
        ISBN.Text = item.Isbn;
        Author.Text = item.Author;
        if (item.ImgPath != null)
        {
            Image.Source = new Bitmap(item.ImgPath);
        }
    }
}