using SettlementService.Domain.Constants;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Domain.Entities;

public class BookingAvailableHour : OfficeHour
{
    private readonly OfficeHour _officeHour;

    public BookingAvailableHour(OfficeHour officeHour)
    {
        _officeHour = officeHour ?? throw new ArgumentNullException(nameof(officeHour));
        StartTime = _officeHour.StartTime;
        EndTime = MilitaryTime.Create(_officeHour.EndTime.Value.AddHours(-1));
    }

    public TimeType StartTime { get; private set; }
    public TimeType EndTime { get; private set; }

    private bool IsValidHours(TimeType startTime, TimeType endTime)
    {
        return startTime >= _officeHour.StartTime &&
               endTime <= MilitaryTime.Create(_officeHour.EndTime.Value.AddHours(-1)) &&
               endTime.IsDurationDifference(startTime, SettlementOptions.ReserveDuration);
    }

    public void UpdateHours(TimeType startTime, TimeType endTime)
    {
        if (IsValidHours(startTime, endTime))
            throw new ArgumentException("Start time must be earlier than end time.");

        StartTime = startTime;
        EndTime = endTime;
    }
}