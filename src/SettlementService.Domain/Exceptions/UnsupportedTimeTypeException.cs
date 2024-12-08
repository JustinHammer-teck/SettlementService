namespace SettlementService.Domain.Exceptions;

public class UnsupportedTimeTypeException : Exception
{
    public UnsupportedTimeTypeException(string message) : base(message)
    {
    }
}