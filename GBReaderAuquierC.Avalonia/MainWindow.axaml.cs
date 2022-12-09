using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Presenter.Views;
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