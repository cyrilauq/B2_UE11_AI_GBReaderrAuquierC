namespace GBReaderAuquierC.Infrastructures.Exceptions
{
    public class UnableToConnectException : Exception
    {
        public UnableToConnectException(string message, Exception e) : base(message, e) {}
    }
}