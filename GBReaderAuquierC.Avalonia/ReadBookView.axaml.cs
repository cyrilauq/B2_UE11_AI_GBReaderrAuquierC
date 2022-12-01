using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Presentation;

namespace GBReaderAuquierC.Avalonia;

public partial class ReadBookView : UserControl, IAskToDisplayMessage, IReadView
{
    public ReadBookView()
    {
        InitializeComponent();
    }

    public void AddListener(IDisplayMessages listener)
    {
        //throw new System.NotImplementedException();
    }

    public void OnEnter(string fromView)
    {
        
    }

    public void SetTitle(string title)
    {
        Title.Text = title;
    }

    public void SetCurrentPage(int nPage, string content)
    {
        NPage.Text = $"Page n° {nPage}:";
        Content.Text = content;
    }
}