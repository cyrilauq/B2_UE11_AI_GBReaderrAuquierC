using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GBReaderAuquierC.Avalonia;

public partial class ReadBookView : UserControl, IAskToDisplayMessage
{
    public ReadBookView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void AddListener(IDisplayMessages listener)
    {
        //throw new System.NotImplementedException();
    }
}