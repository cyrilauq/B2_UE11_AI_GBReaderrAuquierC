using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GBReaderAuquierC.Avalonia;

public partial class ReadBookView : UserControl
{
    public ReadBookView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}