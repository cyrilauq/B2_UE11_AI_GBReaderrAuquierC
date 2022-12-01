namespace GBReaderAuquierC.Avalonia.Views
{
    public interface IReadView
    {
        void SetTitle(string title);

        void SetCurrentPage(int nPage, string content);
    }
}