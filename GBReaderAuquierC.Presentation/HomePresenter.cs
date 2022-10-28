namespace GBReaderAuquierC.Presentation;

public class HomePresenter
{
    private IHomeView _view;
    
    public HomePresenter(IHomeView view)
    {
        _view = view;
        _view.HomePresenter = this;
    }
}

public interface IHomeView
{
    HomePresenter HomePresenter { set; }
}