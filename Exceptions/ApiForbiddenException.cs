namespace Auth0MvcDemo.Exceptions;

public class ApiForbiddenException : Exception
{
    public ApiForbiddenException()
        : base("No tienes permisos para realizar esta operación.")
    {
    }

    public ApiForbiddenException(string message)
        : base(message)
    {
    }
}
