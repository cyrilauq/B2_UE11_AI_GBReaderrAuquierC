using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Infrastructures;
using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Presenter;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Avalonia
{
    public partial class App : Application
    {
        private MainWindow _mainWindow;
        Session _session;
        private IDataRepository _repo;
        private ISessionRepository _sessionRepo;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                _mainWindow = new MainWindow();
                _session = new();
                _sessionRepo = new SessionJsonRepository(
                    Path.Join(Environment.GetEnvironmentVariable("USERPROFILE"), "ue36"), 
                    "e200106-session.json"
                );
                _repo = new BdRepository("MySql.Data.MySqlClient", 
                    new DbInformations("192.168.128.13", "in20b1001", "in20b1001", "4918"));
                desktop.MainWindow = _mainWindow;
                
                // IDataRepository repo = new JsonRepository(Path.Join(Environment.GetEnvironmentVariable("USERPROFILE"), "ue36"), "e200106.json");
            
                ReadBookView readBookView = new();
                var readPresenter = new ReadPresenter(readBookView, _mainWindow, _mainWindow, _sessionRepo);
                
                StatisticsView statisticsView = new();
                var statisticsPresenter = new StatisticsPresenter(statisticsView, _mainWindow, _mainWindow, _sessionRepo);
                
                HomeView homeView = new();
                var homePresenter = new HomePresenter(homeView, _mainWindow, _mainWindow, _sessionRepo, _repo);

                _mainWindow.RegisterView("HomeView", homeView);
                _mainWindow.RegisterView("ReadBookView", readBookView);
                _mainWindow.RegisterView("StatisticsView", statisticsView);
                _mainWindow.GoTo("HomeView");
                
                desktop.MainWindow.Closing += homePresenter.OnSessionSaved;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}