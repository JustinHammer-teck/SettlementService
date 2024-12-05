
namespace SettlementService.Domain.ValueObjects;

public abstract record TimeType
{
    public TimeOnly Time { get; private init; }
    
    protected TimeType(TimeOnly time)
    {
        Time = time;
    }

    private int CompareTo(TimeType? other)
    {
        return other is null ? 1 :
            Time.CompareTo(other.Time);
    }

    public bool IsDurationDifference(TimeOnly time2, TimeSpan duration)
    {
        var difference = (Time.ToTimeSpan() - time2.ToTimeSpan()).Duration();
        return difference == duration;
    }  
    
    public static bool operator <(TimeType left, TimeType right) =>
        left.CompareTo(right) < 0;

    public static bool operator >(TimeType left, TimeType right) =>
        left.CompareTo(right) > 0;

    public static bool operator <=(TimeType left, TimeType right) =>
        left.CompareTo(right) <= 0;

    public static bool operator >=(TimeType left, TimeType right) =>
        left.CompareTo(right) >= 0;
    
    public static implicit operator TimeOnly(TimeType timeType) => timeType.Time;
};

public sealed record MilitaryTime: TimeType
{
    public TimeOnly Time { get; private init; }

    private MilitaryTime(TimeOnly time) : base(time)
    {
        Time = time;
    }
    
    public static MilitaryTime Create(string time) =>
        TimeOnly.TryParseExact(time, "HH:mm", out var parsedTime)
            ? new MilitaryTime(parsedTime)
            : throw new ArgumentException("Invalid time format. Expected 'HH:mm', but received {time}", time);

    public static MilitaryTime Create(TimeOnly time) => new (time);

    public override string ToString() => Time.ToString("HH:mm");
}