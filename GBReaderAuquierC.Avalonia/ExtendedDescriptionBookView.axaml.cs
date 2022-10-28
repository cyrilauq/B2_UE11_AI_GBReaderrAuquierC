using Avalonia.Controls;
using Avalonia.Media.Imaging;
using GBReaderAuquierC.Presentation;

namespace GBReaderAuquierC.Avalonia;

public partial class ExtendedDescriptionBookView : UserControl
{
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
        if (item.ImgPath != null && item.ImgPath.Trim().Length > 0)
        {
            Image.Source = new Bitmap(item.ImgPath);
        }
    }
}