using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Infrastructures.DTO;

namespace GBReaderAuquierC.Infrastructures
{
    public static class Mapper
    {
        /// <summary>
        /// Convertit un BookDTO en Book
        /// </summary>
        /// <param name="dto">BookDTO qu'on veut convertir</param>
        /// <returns>L'équivalent Book du BookDTO donné.</returns>
        public static Book ConvertToBook(BookDTO dto) =>
            dto.Version switch
            {
                "1.1" => FromV1_1(dto),
                "1.2" => FromV1_2(dto),
                _ => FromV1(dto)
            };

        /**
     * Convertit un BookDTO de version 1 vers un objet Book.
     *
     * @return  Un objet Book représentant l'équivalent du BookDTO courant.
     */
        private static Book FromV1(BookDTO dto) 
            => new (
                dto.Title,
                dto.Author,
                Isbn.ConvertForUser(dto.Isbn),
                dto.Resume,
                ""
            );

        /**
     * Convertit un BookDTO de version 1.1 vers un objet Book.
     *
     * @return  Un objet Book représentant l'équivalent du BookDTO courant.
     */
        private static Book FromV1_1(BookDTO dto) 
            => new (
                dto.Title,
                dto.Author,
                Isbn.ConvertForUser(dto.Isbn),
                dto.Resume,
                dto.ImagePath
            );

        /**
     * Convertit un BookDTO de version 1.1 vers un objet Book.
     *
     * @return  Un objet Book représentant l'équivalent du BookDTO courant.
     */
        private static Book FromV1_2(BookDTO dto) {
            var result = new Book(
                dto.Title,
                dto.Author,
                Isbn.ConvertForUser(dto.Isbn),
                dto.Resume,
                dto.ImagePath
            );
            result.Pages = ConvertPages(dto.Pages);
            return result;
        }

        private static IList<Page> ConvertPages(IList<PageDto> dto)
        {
            IList<Page> result = new List<Page>();
            
            foreach (PageDto d in dto)
            {
                result.Add(new Page(d.Content));
            }
            
            // 1. Parcourir les pages
            // 2. Récupérer le DTO de cette page
            // 3. Convertir les choix de ce dernier
            // 4. Ajouter les choix à la page
            
            // TODO : Voir avec le prof si je peux fonctionner différemment du java
            //          ==> Ne pas mémoriser des objet dans les choix mais le numéro des pages cibles
            
            foreach (Page p in result)
            {
                foreach (PageDto d in dto)
                {
                    if (d.Content.Equals(p.Content))
                    {
                        p.Choices = ConvertChoices(d.Choices, result);
                        break;
                    }
                }
            }

            return result;
        }

        private static Dictionary<string, Page> ConvertChoices(Dictionary<string, string> choices, IList<Page> pages)
        {
            Dictionary<string, Page> result = new();
            foreach (string c in choices.Keys)
            {
                result.Add(c, GetPageForContent(choices[c], pages));
            }
            return result;
        }

        private static Page GetPageForContent(string content, IList<Page> pages) 
            => pages.FirstOrDefault(dto => dto.Content.Equals(content));

        public static Session ConvertToSession(SessionDto dto)
        {
            if (dto == null)
            {
                return new Session();
            }
            var history = new Dictionary<string, ReadingSession>();
            foreach (var bsd in dto.History)
            {
                history.Add(bsd.Key, ConvertDtoToSave(bsd.Value));
            }
            Session result = new() { History = history };
            return result;
        }

        public static SessionDto ConvertToDto(Session session) 
            => new (ConvertHistoryToDto(session.History));

        public static SessionDto ConvertToDto(Dictionary<string, ReadingSession> historique)
            => new(ConvertHistoryToDto(historique));

        private static Dictionary<string, BookSaveDto> ConvertHistoryToDto(Dictionary<string, ReadingSession> history)
        {
            var result = new Dictionary<string, BookSaveDto>();

            foreach (var bs in history)
            {
                result.Add(bs.Key, ConvertSaveToDto(bs.Value));
            }

            return result;
        }

        private static BookSaveDto ConvertSaveToDto(ReadingSession readingSession) 
            => new (
                readingSession.Begin.ToString("dd/MM/yyyy hh:mm:ss"), 
                readingSession.LastUpdate.ToString("dd/MM/yyyy hh:mm:ss"), 
                readingSession.History
            );

        private static ReadingSession ConvertDtoToSave(BookSaveDto dto) 
            => ReadingSession.Get(
                string.IsNullOrEmpty(dto.BeginDate) ? DateTime.Now : DateTime.Parse(dto.BeginDate), 
                string.IsNullOrEmpty(dto.LastUpdate) ? DateTime.Now : DateTime.Parse(dto.LastUpdate), 
                dto.History 
            );
    }
}
