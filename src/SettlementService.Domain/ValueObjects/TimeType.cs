
namespace SettlementService.Domain.ValueObjects;

public abstract record TimeType
{
    public TimeOnly Time { get; init; }
    
    protected TimeType(TimeOnly time)
    {
        Time = time;
    }

    public int CompareTo(TimeType? other)
    {
        return other is null ? 1 :
            Time.CompareTo(other.Time);
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

public sealed record MilitaryTime(TimeOnly Time): TimeType(Time)
{
    public static MilitaryTime Create(string time) =>
        TimeOnly.TryParseExact(time, "HH:mm", out var parsedTime)
            ? new MilitaryTime(parsedTime)
            : throw new ArgumentException("Invalid time format. Expected 'HH:mm', but received {time}", time);

    public override string ToString() => Time.ToString("HH:mm");

    public static bool TryParse(string time) =>
        TimeOnly.TryParseExact(time, "HH:mm", out _);
}

public static class Time
{
    public static bool IsOneHourDifference(TimeOnly time1, TimeOnly time2)
    {
        var difference = (time1.ToTimeSpan() - time2.ToTimeSpan()).Duration();
        return difference == TimeSpan.FromHours(1);
    }  
}