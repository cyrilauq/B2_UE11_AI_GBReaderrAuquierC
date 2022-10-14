using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace GBReaderAuquierC.Avalonia
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, UserControl> views = new Dictionary<string, UserControl>();

        public MainWindow()
        {
            InitializeComponent();
            HomeView homeView = new HomeView();
            CreateBookView createBookView = new CreateBookView();
            homeView.Router = GoTo;
            views.Add("LoginView", homeView);
            views.Add("CreateBookView", createBookView);
            Root.Children.Add(homeView);
            Root.Children.Add(createBookView);
        }

        private void GoTo(string view)
        {
            Root.Children.Clear();
            Root.Children.Add(views[view]);
        }
    }
}