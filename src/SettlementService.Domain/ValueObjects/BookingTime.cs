namespace SettlementService.Domain.ValueObjects;

public class BookingTime
{
    public TimeType Time { get; private set; } 
    public int Hour { get; private set; }
    public TimeOnly HeldTo => Time.Value.AddMinutes(59);
    private BookingTime()
    {
    }

    public BookingTime(TimeType time)
    {
        Time = time;
        Hour = time.Value.Hour;
    }
}
