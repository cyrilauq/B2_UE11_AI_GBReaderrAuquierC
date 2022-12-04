using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GBReaderAuquierC.Presenter.ViewModel;

namespace GBReaderAuquierC.Avalonia.Pages
{
    public partial class PageChoiceView : UserControl
    {
        private ChoiceViewModel _choice;
        
        public int NumTarget { get => _choice.NTarget; }
        public string ContentTarget { get => _choice.Text; }

        public ChoiceViewModel Choice
        {
            set
            {
                _choice = value;
                Content.Text = $"{value.Text}: aller vers la page {value.NTarget}.";
            }
        }

        public PageChoiceView()
        {
            InitializeComponent();
        }
    }
}