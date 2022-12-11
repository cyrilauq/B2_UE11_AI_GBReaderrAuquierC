using GBReaderAuquierC.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBReaderAuquierC.Repositories.DTO;
using Org.BouncyCastle.Security;

namespace GBReaderAuquierC.Repositories
{
    public class Mapper
    {
        /// <summary>
        /// Convertit un BookDTO en Book
        /// </summary>
        /// <param name="dto">BookDTO qu'on veut convertir</param>
        /// <returns>L'équivalent Book du BookDTO donné.</returns>
        public static Book ConvertToBook(BookDTO dto) {
            switch (dto.Version) {
                case "1.1":
                    return FromV1_1(dto);
                case "1.2":
                    return FromV1_2(dto);
                case "1":
                default:
                    return FromV1(dto);
            }
        }

        /**
     * Convertit un BookDTO de version 1 vers un objet Book.
     *
     * @return  Un objet Book représentant l'équivalent du BookDTO courant.
     */
        private static Book FromV1(BookDTO dto) {
            return new Book(
                dto.Title,
                dto.Author,
                ConvertDtoISBNToBook(dto.ISBN),
                dto.Resume,
                ""
            );
        }

        /**
     * Convertit un BookDTO de version 1.1 vers un objet Book.
     *
     * @return  Un objet Book représentant l'équivalent du BookDTO courant.
     */
        private static Book FromV1_1(BookDTO dto) {
            return new Book(
                dto.Title,
                dto.Author,
                ConvertDtoISBNToBook(dto.ISBN),
                dto.Resume,
                dto.ImagePath
            );
        }

        /**
     * Convertit un BookDTO de version 1.1 vers un objet Book.
     *
     * @return  Un objet Book représentant l'équivalent du BookDTO courant.
     */
        private static Book FromV1_2(BookDTO dto) {
            var result = new Book(
                dto.Title,
                dto.Author,
                ConvertDtoISBNToBook(dto.ISBN),
                dto.Resume,
                dto.ImagePath
            );
            result.Pages = ConvertPages(dto.Pages);
            return result;
        }

        private static IList<Page> ConvertPages(IList<PageDTO> dto)
        {
            IList<Page> result = new List<Page>();
            
            foreach (PageDTO d in dto)
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
                foreach (PageDTO d in dto)
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

        private static string ConvertDtoISBNToBook(string isbn)
        {
            if (isbn.Replace("-", "").Length == 11)
            {
                
                if (isbn.EndsWith("10"))
                {
                    return isbn.Substring(0, isbn.Length - 2) + "X";
                }
                return isbn.Substring(0, isbn.Length - 2) + "0";
            }
            return isbn;
        }

        private static Page GetPageForContent(string content, IList<Page> pages)
        {
            foreach (Page dto in pages)
            {
                if (dto.Content.Equals(content))
                {
                    return dto;
                }
            }

            return null;
        }

        public static Session ConvertToSession(SessionDTO dto)
        {
            if (dto == null)
            {
                return new Session();
            }
            var history = new Dictionary<string, BookSave>();
            foreach (var bsd in dto.History)
            {
                history.Add(bsd.Key, ConvertDTOToSave(bsd.Value));
            }
            Session result = new();
            result.History = history;
            return result;
        }

        public static SessionDTO ConvertToDTO(Session session)
        {
            return new SessionDTO(ConvertHistoryToDTO(session.History));
        }

        private static Dictionary<string, BookSaveDTO> ConvertHistoryToDTO(Dictionary<string, BookSave> history)
        {
            var result = new Dictionary<string, BookSaveDTO>();

            foreach (var bs in history)
            {
                result.Add(bs.Key, ConvertSaveToDTO(bs.Value));
            }

            return result;
        }

        private static BookSaveDTO ConvertSaveToDTO(BookSave bookSave)
        {
            return new BookSaveDTO(
                bookSave.Begin.ToString("dd/MM/yyyy hh:mm:ss"), 
                bookSave.LastUpdate.ToString("dd/MM/yyyy hh:mm:ss"), 
                bookSave.History
            );
        }

        private static BookSave ConvertDTOToSave(BookSaveDTO dto)
        {
            return BookSave.Get(
                dto.BeginDate == null || dto.BeginDate.Length == 0 ? DateTime.Now : DateTime.Parse(dto.BeginDate), 
                dto.LastUpdate == null || dto.LastUpdate.Length == 0 ? DateTime.Now : DateTime.Parse(dto.LastUpdate), 
                dto.History 
            );
        }
    }
}
