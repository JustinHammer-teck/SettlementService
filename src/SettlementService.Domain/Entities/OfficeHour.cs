using SettlementService.Domain.Constants;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Domain.Entities;

public class OfficeHour
{
    public OfficeHour()
    {
        StartTime = MilitaryTime.Create(new TimeOnly(9, 0));
        EndTime = MilitaryTime.Create(new TimeOnly(17, 0));
    }

    public TimeType StartTime { get; private set; }
    public TimeType EndTime { get; private set; }

    public void UpdateHours(TimeType startTime, TimeType endTime)
    {
        if (startTime >= endTime &&
            endTime.IsDurationDifference(startTime, SettlementOptions.ReserveDuration))
            throw new ArgumentException("Office Hour should be valid");

        StartTime = startTime;
        EndTime = endTime;
    }
}