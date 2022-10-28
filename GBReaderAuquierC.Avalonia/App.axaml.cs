using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace GBReaderAuquierC.Avalonia
{
    public partial class App : Application
    {
        private readonly MainWindow _mainWindow = new MainWindow();
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                _mainWindow.Opened += OnWindowOpened;
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public void OnWindowOpened(object? sender, EventArgs args)
        {
            
        }
    }
}