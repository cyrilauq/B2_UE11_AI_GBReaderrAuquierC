using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace GBReaderAuquierC.Avalonia;

public partial class DescriptionBookView : UserControl
{
    private TextBlock _title;
    private TextBlock _author;
    private TextBlock _isbn;
    private Image _image;
    private TextBlock _resume;
    
    private string _resumeTxt;

    private bool _showDetails = true;

    public DescriptionBookView()
    {
        InitializeComponent();
    }
    
    public void SetBookInfo(string title, string isbn, string author, string resume, string imagePath)
    {
        Title.Text = title;
        Author.Text = author;
        Isbn.Text = isbn;
        if (imagePath != null && isbn.Trim().Length > 0)
        {
            Image.Source = new Bitmap(imagePath);
        }
    }

    private void DisplayResume(object? sender, PointerEventArgs e)
    {
        var resumeBlock = this.FindControl<TextBlock>("Resume");
        resumeBlock.Text = _resumeTxt;
        resumeBlock.IsVisible = true;
    }

    private void DisplayDetails(object? sender, PointerPressedEventArgs e)
    {
        if (_showDetails)
        {
            
        }
        else
        {
            DisplayResume(sender, e);
        }
    }
}