﻿using GBReaderAuquierC.Domains;

namespace GBReaderAuquierC.Infrastructures;

public interface IDataRepository
{
    // TODO : Créer base de données MySQL server ==> pour les tests
    
    // TODO : Ne pas donner des int mais peut-être un RangeArg (contient début et fin de la liste à extraire)
    public IEnumerable<Book> GetBooks(int begin = 0, int end = 0);

    // TODO : Créer un orgument FilterArg, avec le type de filtre et la recherche
    public Book Search(string isbn);
    
    // TODO : Créer un méthode loadBook(string isbn) qui chargera toutes les données du livre ayant l'isbn donné.
    public Book LoadBook(string isbn);

    public IEnumerable<Book> SearchBooks(string search, SearchOption Option, RangeArg arg = null);
}