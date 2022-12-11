using GBReaderAuquierC.Domains;

namespace GBReaderAuquierC.Presenter.Views;

public interface IView
{

    public Session Session{ set; get; }
    
    public void GoTo(string toView);

    public void OnEnter(string fromView);
}