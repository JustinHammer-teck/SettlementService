namespace SettlementService.Domain.ValueObjects;

public abstract record NameType;

public record FullName : NameType
{
    public string FirstName { get; private init; }
    public string LastName { get; private init; }
    
    public FullName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException($"'{nameof(firstName)}' cannot be null or whitespace.", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException($"'{nameof(lastName)}' cannot be null or whitespace.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }

    public static FullName Create(string name) =>
        name.Split(" ") switch
        {
            [var firstName, var lastName] => new FullName(firstName, lastName),
            _ => throw new ArgumentException($"Invalid Format of full name")
        };
}