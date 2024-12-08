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

    public static bool IsValid(string name, out string[] nameParts)
    {
        nameParts = name?.Split(" ") ?? [];

        return !string.IsNullOrWhiteSpace(name) &&
               nameParts.Length > 1 &&
               nameParts.All(part => !string.IsNullOrWhiteSpace(part));
    }

    public static bool TryCreate(string name, out FullName fullName)
    {
        if (IsValid(name, out var nameParts))
        {
            fullName = new FullName(nameParts[0], nameParts[1]);
            return true;
        }

        fullName = default;
        return false;
    }

    public static FullName Create(string name)
    {
        if (IsValid(name, out var nameParts))
            return new FullName(nameParts[0], nameParts[1]);

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