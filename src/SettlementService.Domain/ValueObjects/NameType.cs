namespace SettlementService.Domain.ValueObjects;

public abstract record NameType;

public record FullName : NameType
{
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; private init; } = string.Empty;
    public string LastName { get; private init; } = string.Empty;

    public static bool TryCreate(string name, out FullName fullName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

        var partialName = name.Split(" ");

        if (partialName.Length < 2)
        {
            fullName = default;
            return false;
        }

        fullName = new FullName(partialName[0], partialName[1]);
        return true;
    }

    public static FullName Create(string name)
    {
        if (TryCreate(name, out var fullName))
            return fullName;

        throw new ArgumentException($"Invalid full name format: {name}");
    }

    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException($"'{nameof(firstName)}' cannot be null or whitespace.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException($"'{nameof(lastName)}' cannot be null or whitespace.", nameof(lastName));

        return new FullName(firstName, lastName);
    }
}