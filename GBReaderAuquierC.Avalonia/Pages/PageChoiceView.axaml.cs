using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GBReaderAuquierC.Avalonia.Views;
using GBReaderAuquierC.Presenter.ViewModel;

namespace GBReaderAuquierC.Avalonia.Pages
{
    public partial class PageChoiceView : UserControl
    {
        private ChoiceViewModel _choice;
        
        public int NumTarget { get => _choice.NTarget; }
        public string ContentTarget { get => _choice.Text; }

        public event EventHandler<GotToPageEventArgs> Click;

        public ChoiceViewModel Choice
        {
            set
            {
                _choice = value;
                Content.Content = $"{value.Text}: aller vers la page {value.NTarget}.";
                Content.Click += OnClick;
            }
        }

        private void OnClick(object? sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, new GotToPageEventArgs(_choice.NTarget, _choice.Text));
        }

        public PageChoiceView()
        {
            InitializeComponent();
        }
    }
}