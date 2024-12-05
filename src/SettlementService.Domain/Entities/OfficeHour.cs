using SettlementService.Domain.ValueObjects;

namespace SettlementService.Domain.Entities;

public class OfficeHour
{
    public TimeType StartTime { get; private set; }
    public TimeType EndTime { get; private set; }

    protected OfficeHour()
    {
        StartTime = new MilitaryTime(new TimeOnly(9, 0));
        EndTime = new MilitaryTime(new TimeOnly(17, 0));
    }

    public void UpdateHours(TimeType startTime, TimeType endTime)
    {
        if (startTime >= endTime && Time.IsOneHourDifference(endTime, startTime))
            throw new ArgumentException("Start time must be earlier than end time.");

        StartTime = startTime;
        EndTime = endTime;
    }
}

public class BookingAvailableHour : OfficeHour
{
    private readonly OfficeHour _officeHour;

    public TimeType StartTime { get; private set; }
    public TimeType EndTime { get; private set; }

    public BookingAvailableHour(OfficeHour officeHour)
    {
        _officeHour = officeHour ?? throw new ArgumentNullException(nameof(officeHour));
        StartTime = _officeHour.StartTime;
        EndTime = _officeHour.EndTime;
    }
    
    public bool CanBeReserve(TimeOnly time) => 
        StartTime <= time && time <= new TimeOnly(16, 00);
    
    private bool IsValidHours(TimeType startTime, TimeType endTime) =>
        startTime >= new TimeOnly(9, 00) &&
        endTime <= new TimeOnly(16, 00) &&
        Time.IsOneHourDifference(endTime, startTime);
    public void UpdateHours(TimeType startTime, TimeType endTime)
    {
        if (IsValidHours(startTime,endTime))
            throw new ArgumentException("Start time must be earlier than end time.");

        StartTime = startTime;
        EndTime = endTime;
    }
}



