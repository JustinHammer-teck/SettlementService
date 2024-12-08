using System.Linq.Expressions;

namespace SettlementService.Domain.Exceptions;

public class UnsupportedNameTypeException : Exception
{
    public UnsupportedNameTypeException(string message) : base(message)
    {
    } 
}