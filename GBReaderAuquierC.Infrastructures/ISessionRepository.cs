using System.ComponentModel;
using GBReaderAuquierC.Domains;

namespace GBReaderAuquierC.Infrastructures
{
    public interface ISessionRepository : INotifyPropertyChanged
    {
        Book CurrentBook { get; set; }
        Book ReadingBook { get; set; }
        Page ReadingPage { get; set; }
        Dictionary<string, BookSave> History { get; }

        void LoadSession();

        void SaveSession();
    }
}