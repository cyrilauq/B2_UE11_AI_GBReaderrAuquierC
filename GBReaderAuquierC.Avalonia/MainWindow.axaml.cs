using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Avalonia
{
    public partial class MainWindow : Window, IDisplayMessages, IBrowseViews
    {
        private Dictionary<string, UserControl> _views = new Dictionary<string, UserControl>();
        private readonly WindowNotificationManager _notificationManager;
        private readonly Session _session;

        public MainWindow()
        {
            InitializeComponent();
            _session = new Session();
            _notificationManager = new WindowNotificationManager(this);
        }

        public void RegisterView(string viewName, UserControl view) 
            => _views[viewName] = view;

        private void CreateViews()
        {
            // IDataRepository repo = new JsonRepository(Path.Join(Environment.GetEnvironmentVariable("USERPROFILE"), "ue36"), "e200106.json");
            IDataRepository repo = new BDRepository("MySql.Data.MySqlClient", "server=192.168.128.13;database=in20b1001;uid=in20b1001;pwd=4918");
            HomeView homeView = new();
            homeView.Session = _session;
            homeView.AddListener(this);
            var homePresenter = new HomePresenter(homeView, this, this, _session, repo);
            
            ReadBookView readBookView = new();
            _views.Add("HomeView", homeView);
            _views.Add("ReadBookView", readBookView);
            GoTo("HomeView");
        }

        public void GoTo(string view)
        {
            var found = Found(view);
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
            if (IsVisible)
            {
                _notificationManager.Show(notification);
            }
        }

        public void DisplayNotification(NotifInfo info) =>
            DisplayNotification(new Notification(
                info.Title,
                info.Content
            ));
    }
}