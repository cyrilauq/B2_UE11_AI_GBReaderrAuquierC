using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GBReaderAuquierC.Avalonia;

public partial class CreateBookView : UserControl
{
    public CreateBookView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}