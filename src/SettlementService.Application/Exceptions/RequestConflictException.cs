namespace SettlementService.Application.Exceptions;

public class RequestConflictException : Exception
{
    public RequestConflictException(string command, string type, string message) : base(message)
    {
        Command = command;
        Type = type;
        Message = message;
    }

    public string Command { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
}