using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Presentation;
using GBReaderAuquierC.Presenter;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Avalonia
{
    public partial class App : Application
    {
        private MainWindow _mainWindow;
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                _mainWindow = new MainWindow();
                desktop.MainWindow = _mainWindow;
                
                // IDataRepository repo = new JsonRepository(Path.Join(Environment.GetEnvironmentVariable("USERPROFILE"), "ue36"), "e200106.json");
                IDataRepository repo = new BDRepository("MySql.Data.MySqlClient", 
                    new DbInformations("192.168.128.13", "in20b1001", "in20b1001", "4918"));
                Session session = new();
            
                ReadBookView readBookView = new();
                var readPresenter = new ReadPresenter(readBookView, _mainWindow, _mainWindow, session, repo);
                
                HomeView homeView = new();
                var homePresenter = new HomePresenter(homeView, _mainWindow, _mainWindow, session, repo);

                _mainWindow.RegisterView("HomeView", homeView);
                _mainWindow.RegisterView("ReadBookView", readBookView);
                _mainWindow.GoTo("HomeView");
            }

            base.OnFrameworkInitializationCompleted();
        }

        public void OnWindowOpened(object? sender, EventArgs args)
        {
            
        }
    }
}