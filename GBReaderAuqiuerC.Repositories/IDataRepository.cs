﻿using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Domains.Repository;

namespace GBReaderAuquierC.Presenter;

public interface IDataRepository
{
    public List<BookDTO> GetData();

    public Book Search(string isbn);
}