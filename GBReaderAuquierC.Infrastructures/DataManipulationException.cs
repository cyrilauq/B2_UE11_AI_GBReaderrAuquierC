namespace GBReaderAuquierC.Infrastructures
{
    public class DataManipulationException : Exception
    {
        public DataManipulationException(string message, Exception origine) : base(message, origine)
        {
            
        }
    }
}