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
            ReadBookView readBookView = new ReadBookView();
            homeView.Router = GoTo;
            views.Add("HomeView", homeView);
            views.Add("CreateBookView", readBookView);
            Root.Children.Add(homeView);
            Root.Children.Add(readBookView);
        }

        private void GoTo(string view)
        {
            var found = Found(view);
            Root.Children.Clear();
            Root.Children.Add(found);
        }

        private UserControl Found(string view)
        {
            if (!views.ContainsKey(view))
            {
                throw new ArgumentException("The view [" + view + "] doesn't exist.");
            }

            return views[view];
        }
    }
}