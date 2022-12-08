namespace GBReaderAuquierC.Repositories.Exceptions
{
    public class UnableToConnectException : Exception
    {
        public UnableToConnectException(string message, Exception e) : base(message, e) {}
    }
}