using SettlementService.Domain.Exceptions;

namespace SettlementService.Domain.ValueObjects;

public abstract record TimeType
{
    protected TimeType(TimeOnly value)
    {
        Value = value;
    }

    public TimeOnly Value { get; }

    public bool IsDurationDifference(TimeOnly time2, TimeSpan duration)
    {
        var difference = (Value.ToTimeSpan() - time2.ToTimeSpan()).Duration();
        return difference == duration;
    }

    /*
     * I like to make object to be used as fluently as possible
     * these customer operate would help tremendously.
     * In context of building a good framework think we should try to design to help
     * developer do there work faster with this type of design.
     */

    private int CompareTo(TimeType? other)
    {
        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    public static bool operator <(TimeType left, TimeType right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(TimeType left, TimeType right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(TimeType left, TimeType right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(TimeType left, TimeType right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static implicit operator TimeOnly(TimeType timeType)
    {
        return timeType.Value;
    }
}

/*
 * From Author: EUGENE
 * I could have enforced the type of Time Format more by design another ValueObject but that
 * would be too over-kill for this demo project.
 * This Type of ValueObject is to show my skill on designing a rich domain object that
 * extendable and easy to maintain.
 */
public sealed record MilitaryTime : TimeType
{
    private MilitaryTime(TimeOnly value) : base(value)
    {
        Value = value;
    }

    public static string DefaultFormat => "HH:mm";
    public static IReadOnlyCollection<string> Formats => [DefaultFormat, "H:mm"];

    public TimeOnly Value { get; }

    public static MilitaryTime Create(TimeOnly time)
    {
        return new MilitaryTime(time);
    }

    public static MilitaryTime Create(string time)
    {
        if (TryCreate(time, out var militaryTime))
            return militaryTime;

        throw new UnsupportedTimeTypeException(
            $"Invalid time format. Expected {Formats.Select(x => x)}, but received {time}");
    }

    public static bool TryCreate(string time, out MilitaryTime militaryTime)
    {
        militaryTime = default;

        if (string.IsNullOrEmpty(time))
            throw new ArgumentException($"'{nameof(time)}' cannot be null or whitespace.", nameof(time));

        foreach (var format in Formats)
            if (TimeOnly.TryParseExact(time, format, out var parsedTime))
            {
                militaryTime = new MilitaryTime(parsedTime);
                return true;
            }

        return false;
    }

    public override string ToString()
    {
        return Value.ToString(DefaultFormat);
    }

    public string ToShortForm()
    {
        return Value.ToString("H:mm");
    }
}