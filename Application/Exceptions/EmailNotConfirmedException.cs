using System;

namespace Application.Exceptions
{
    public class EmailNotConfirmedException : Exception
    {
        public EmailNotConfirmedException()
            : base("Email is not confirmed.") { }

        public EmailNotConfirmedException(string message)
            : base(message) { }

        public EmailNotConfirmedException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
