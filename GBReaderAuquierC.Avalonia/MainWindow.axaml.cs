using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Presentation;

namespace GBReaderAuquierC.Avalonia
{
    public interface IDisplayMessages
    {
        void DisplayNotification(Notification notification);
    }
    public partial class MainWindow : Window, IDisplayMessages, IBrowseViews
    {
        private Dictionary<string, UserControl> _views = new Dictionary<string, UserControl>();
        private readonly WindowNotificationManager _notificationManager;
        private readonly Session _session;

        public MainWindow()
        {
            _session = new Session();
            _notificationManager = new WindowNotificationManager(this);
            InitializeComponent();
            CreateViews();
        }

        private void CreateViews()
        {
            HomeView homeView = new();
            homeView.Session = _session;
            homeView.AddListener(this);
            ReadBookView readBookView = new();
            readBookView.Session = _session;
            readBookView.AddListener(this);
            homeView.Router = GoTo;
            _views.Add("HomeView", homeView);
            _views.Add("ReadBookView", readBookView);
            GoTo("HomeView");
        }

        public void GoTo(string view)
        {
            var found = Found(view);
            (found as IView).OnEnter("");
            Content = _views[view];
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
            _notificationManager.Show(notification);
        }
    }
}