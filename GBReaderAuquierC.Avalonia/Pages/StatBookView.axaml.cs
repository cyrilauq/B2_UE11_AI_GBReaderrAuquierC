using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using GBReaderAuquierC.Presenter.ViewModel;

namespace GBReaderAuquierC.Avalonia.Pages
{
    public partial class StatBookView : UserControl
    {
        public StatViewModel Info
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("The given informations should not be null.");
                }

                if (value.ImgPath != null)
                {
                    Cover.Source = new Bitmap(value.ImgPath);
                }
                Begin.Text = value.BeginDate;
                LastUpdate.Text = value.LastUpdate;
                Title.Text = value.Title;
            }
        }
    
        public StatBookView()
        {
            InitializeComponent();
        }
    }
}