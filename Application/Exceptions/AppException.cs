namespace Application.Exceptions
{
    public class AppException : Exception
    {
        public AppException() : base("An error has occured!") { }
        public AppException(string message) : base(message) { }
        public AppException(string? message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
