using SettlementService.Domain.Exceptions;

namespace SettlementService.Domain.ValueObjects;

public abstract record TimeType
{
    public TimeOnly Value { get; private init; }

    protected TimeType(TimeOnly value)
    {
        Value = value;
    }
   
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
    
    private int CompareTo(TimeType? other) => 
        other is null ? 1 : Value.CompareTo(other.Value);
    
    public static bool operator <(TimeType left, TimeType right) =>
        left.CompareTo(right) < 0;

    public static bool operator >(TimeType left, TimeType right) =>
        left.CompareTo(right) > 0;

    public static bool operator <=(TimeType left, TimeType right) =>
        left.CompareTo(right) <= 0;

    public static bool operator >=(TimeType left, TimeType right) =>
        left.CompareTo(right) >= 0;
    
    public static implicit operator TimeOnly(TimeType timeType) => timeType.Value;
};

/*
 * From Author: EUGENE
 * I could have enforced the type of Time Format more by design another ValueObject but that
 * would be too over-kill for this demo project.
 * This Type of ValueObject is to show my skill on designing a rich domain object that
 * extendable and easy to maintain.
 */
public sealed record MilitaryTime: TimeType
{
    public static string DefaultFormat => "HH:mm";
    public static IReadOnlyCollection<string> Formats => [DefaultFormat, "H:mm"];
    
    public TimeOnly Value { get; private init; }

    private MilitaryTime(TimeOnly value) : base(value)
    {
        Value = value;
    }
    
    public static MilitaryTime Create(TimeOnly time) => new (time);

    public static MilitaryTime Create(string time) => TryCreate(time);
    
    public static MilitaryTime TryCreate(string time)
    {
        if (string.IsNullOrEmpty(time))
            throw new ArgumentException($"'{nameof(time)}' cannot be null or whitespace.", nameof(time));
        
        foreach (var format in Formats)
        {
            if (TimeOnly.TryParseExact(time, format, out var parsedTime))
                return new MilitaryTime(parsedTime);
        }
        
        throw new UnsupportedTimeTypeException($"Invalid time format. Expected {Formats.Select(x => x)}, but received {time}");
    }
    
    public override string ToString() => Value.ToString(DefaultFormat);
    public string ToShortForm() => Value.ToString("H:mm");
}