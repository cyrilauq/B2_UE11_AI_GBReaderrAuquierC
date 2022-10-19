using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal class Mapper
    {
        /**
     * Convertit un BookDTO en Book
     *
     * @param dto   BookDTO qu'on veut convertir
     *
     * @return      L'équivalent Book du BookDTO donnée.
     */
        public static Book convertToBook(BookDTO dto) {
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

        /**
     * Convertit un Book en BookDTO
     *
     * @param b     Book qu'on veut convertir
     *
     * @return      L'équivalent BookDTO du Book donnée.
     */
        public static BookDTO convertToBookDTO(Book b) {
            return new BookDTO(
                b.Title,
                b.Resume,
                b.Author,
                b.ISBN,
                b.Image,
                "1.1"
            );
        }
    }
}
