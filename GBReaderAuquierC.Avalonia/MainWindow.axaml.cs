using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace GBReaderAuquierC.Avalonia
{
    public interface IDisplayMessages
    {
        void DisplayNotification(Notification notification);
    }
    public partial class MainWindow : Window, IDisplayMessages
    {
        private Dictionary<string, UserControl> _views = new Dictionary<string, UserControl>();
        private readonly WindowNotificationManager _lala;

        public MainWindow()
        {
            _lala = new WindowNotificationManager(this);
            InitializeComponent();
            HomeView homeView = new HomeView();
            homeView.AddListener(this);
            ReadBookView readBookView = new ReadBookView();
            readBookView.AddListener(this);
            homeView.Router = GoTo;
            _views.Add("HomeView", homeView);
            _views.Add("CreateBookView", readBookView);
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
            if (!_views.ContainsKey(view))
            {
                throw new ArgumentException("The view [" + view + "] doesn't exist.");
            }

            return _views[view];
        }

        public void DisplayNotification(Notification notification)
        {
            _lala.Show(notification);
        }
    }
}