namespace SettlementService.Domain.ValueObjects;

public class BookingTime
{
    private BookingTime()
    {
    }

    public BookingTime(TimeType time)
    {
        Time = time;
        Hour = time.Value.Hour;
    }

    public TimeType Time { get; }
    public int Hour { get; private set; }
    public TimeOnly HeldTo => Time.Value.AddMinutes(59);
}