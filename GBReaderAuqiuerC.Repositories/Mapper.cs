using GBReaderAuquierC.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBReaderAuquierC.Repositories
{
    public class Mapper
    {
        /**
     * Convertit un BookDTO en Book
     *
     * @param dto   BookDTO qu'on veut convertir
     *
     * @return      L'équivalent Book du BookDTO donnée.
     */
        public static Book ConvertToBook(BookDTO dto) {
            switch (dto.Version) {
                case "1.1":
                    return FromV1_1(dto);
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
                dto.ISBN,
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
                dto.ISBN,
                dto.Resume,
                dto.ImagePath
            );
        }
    }
}
