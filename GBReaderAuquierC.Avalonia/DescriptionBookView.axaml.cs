using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace GBReaderAuquierC.Avalonia;

public partial class DescriptionBookView : UserControl
{
    private TextBlock _title;
    private TextBlock _author;
    private TextBlock _isbn;
    private Image _image;
    
    private string _resumeTxt;

    public DescriptionBookView()
    {
        InitializeComponent();
        InitComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void InitComponent()
    {
        _title = this.FindControl<TextBlock>("Title");
        _isbn = this.FindControl<TextBlock>("Isbn");
        _author = this.FindControl<TextBlock>("Author");
        _image = this.FindControl<Image>("Image");
    }

    public void SetBookInfo(string title, string isbn, string author, string resume, string imagePath)
    {
        _title.Text = title;
        _author.Text = author;
        _isbn.Text = isbn;
        _resumeTxt = resume;
        if (imagePath != null && isbn.Trim().Length > 0)
        {
            _image.Source = new Bitmap(imagePath);
        }
    }

    private void DisplayResume(object? sender, PointerEventArgs e)
    {
        var main = this.FindControl<Grid>("Details");
        main.IsVisible = false;
        var resumeBlock = this.FindControl<TextBlock>("Resume");
        resumeBlock.Text = _resumeTxt;
        resumeBlock.IsVisible = true;
    }

    private void DisplayDetails(object? sender, PointerEventArgs e)
    {
        var main = this.FindControl<Grid>("Details");
        main.IsVisible = true;
        var resumeBlock = this.FindControl<TextBlock>("Resume");
        resumeBlock.IsVisible = false;
    }
}