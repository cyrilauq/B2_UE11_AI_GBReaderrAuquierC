using GBReaderAuquierC.Avalonia;

namespace GBReaderAuquierC.Presenter
{
    public class Presenter
    {
        private IBrowseViews _router;
        private IDisplayMessages _notificationManager;
        
        public Presenter(IDisplayMessages notificationManager, IBrowseViews router) 
        {
            _router = router ?? throw new ArgumentNullException(nameof(router));
            _notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
        }
    }
}