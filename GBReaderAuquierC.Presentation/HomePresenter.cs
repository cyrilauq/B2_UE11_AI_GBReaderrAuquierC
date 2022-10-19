namespace GBReaderAuquierC.Presenter;

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