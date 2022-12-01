namespace GBReaderAuquierC.Avalonia
{
    public interface IDisplayMessages
    {
        void DisplayNotification(NotifInfo info);
    }

    public record NotifInfo(string Title, string Content, NotifSeverity Severity);

    public enum NotifSeverity
    {
        Info, 
        Error
    }
}