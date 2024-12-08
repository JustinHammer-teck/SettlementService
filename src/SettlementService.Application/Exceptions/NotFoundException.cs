namespace SettlementService.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : 
        base("Unable to found the requested resource")
    {
    } 
}