namespace GBReaderAuquierC.Avalonia
{
    public interface IBrowseViews
    {
        /// <summary>
        /// Permet de faire le switch d'une vue vers une autre vue.
        /// </summary>
        /// <param name="viwName">Nom de la vue qu'on veut afficher.</param>
        void GoTo(string viwName);
    }
}