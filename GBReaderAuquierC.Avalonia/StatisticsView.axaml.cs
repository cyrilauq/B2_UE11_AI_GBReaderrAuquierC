using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GBReaderAuquierC.Avalonia.Pages;
using GBReaderAuquierC.Presentation.Views;
using GBReaderAuquierC.Presenter.ViewModel;

namespace GBReaderAuquierC.Avalonia
{
    public partial class StatisticsView : UserControl, IStatisticsView
    {
        public event EventHandler GoHomeRequested;
        public IEnumerable<StatViewModel> Books
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("The given value shouldn't be null.");
                }
                StatBooks.Children.Clear();
                foreach (var svm in value)
                {
                    StatBooks.Children.Add(new StatBookView{ Info = svm });
                }
            }
        }

        public int ReadingBookCount
        {
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The count of books must be a positive number");
                }
                BookCount.Text = $"Nombre de livre en cours de lecture: {value}.";
            }
        }

        public StatisticsView()
        {
            InitializeComponent();
        }

        private void OnGoHomeClicked(object? sender, RoutedEventArgs e)
        {
            GoHomeRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}