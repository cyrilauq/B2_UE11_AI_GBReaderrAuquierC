using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Presentation;

namespace GBReaderAuquierC.Avalonia;

public partial class ReadBookView : UserControl, IView, IAskToDisplayMessage
{
    private Session _session;

    public Session Session
    {
        set => _session = value;
        get => _session;
    }
    
    public ReadBookView()
    {
        InitializeComponent();
    }

    public void AddListener(IDisplayMessages listener)
    {
        //throw new System.NotImplementedException();
    }

    public void GoTo(string toView) => throw new System.NotImplementedException();

    public void OnEnter(string fromView)
    {
        Title.Text = _session?.CurrentBook;
    }
}