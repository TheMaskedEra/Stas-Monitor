namespace Stas.Monitor.Domains.Repository.Exceptions;

public class CannotRetrieveAlertException : Exception
{
    public CannotRetrieveAlertException(string message) : base(message)
    {
    }
}
