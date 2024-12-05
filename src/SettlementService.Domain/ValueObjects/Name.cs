namespace SettlementService.Domain.ValueObjects;

public abstract record NameType;

public record FullName : NameType
{
    public string FirstName { get; private init; }
    public string LastName { get; private init; }
    
    private FullName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException($"'{nameof(firstName)}' cannot be null or whitespace.", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException($"'{nameof(lastName)}' cannot be null or whitespace.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }
    
    public static FullName Create(string firstName, string lastName) =>
        new (firstName, lastName);
}

public record MononymName : NameType
{
    public string Value { get; private init; }

    private MononymName(string name) => Value = name;

    public static MononymName Create(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name))
            : new(name);
}