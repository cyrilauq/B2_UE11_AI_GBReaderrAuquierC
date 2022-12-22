namespace GBReaderAuquierC.Infrastructures.Exceptions
{
    public class DataManipulationException : Exception
    {
        public DataManipulationException(string message, Exception origine) : base(message, origine) {}
        public DataManipulationException(string message) : base(message) {}
    }
}