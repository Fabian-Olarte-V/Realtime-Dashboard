namespace Application.Exceptions
{
    public abstract class ApplicationException : Exception
    {
        protected ApplicationException(string message) : base(message) { }

        protected ApplicationException(string message, Exception innerException)
            : base(message, innerException) { }
    }


    public class EntityNotFoundException : ApplicationException
    {
        public string EntityName { get; }
        public object? Key { get; }

        public EntityNotFoundException(string entityName)
            : base($"{entityName} was not found.")
        {
            EntityName = entityName;
        }

        public EntityNotFoundException(string entityName, object key)
            : base($"{entityName} with identifier '{key}' was not found.")
        {
            EntityName = entityName;
            Key = key;
        }
    }

    public class InvalidCredentialsException : ApplicationException
    {
        public InvalidCredentialsException()
            : base("Invalid credentials.") { }
    }

    public class UnauthorizedActionException : ApplicationException
    {
        public UnauthorizedActionException()
            : base("You are not authorized to perform this action.") { }

        public UnauthorizedActionException(string message)
            : base(message) { }
    }

    public class ConcurrencyConflictException : ApplicationException
    {
        public ConcurrencyConflictException()
            : base("The resource was modified by another process. Refresh and try again.") { }

        public ConcurrencyConflictException(string message)
            : base(message) { }
    }

    public class InvalidOperationApplicationException : ApplicationException
    {
        public InvalidOperationApplicationException(string message)
            : base(message) { }
    }

    public class InvalidRequestException : ApplicationException
    {
        public InvalidRequestException(string message)
            : base(message) { }
    }
}
