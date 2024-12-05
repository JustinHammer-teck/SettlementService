using SettlementService.Domain.Entities;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Application.Services.Time;

public class TimeValidationService
{
    private TimeType _validationContext;
    private bool _isValid;
    
    private readonly OfficeHour _officeHour;
    private readonly BookingAvailableHour _bookingAvailableHour;
    public TimeValidationService(
        OfficeHour officeHour,
        BookingAvailableHour bookingAvailableHour)
    {
        _officeHour = officeHour;
        _bookingAvailableHour = bookingAvailableHour;
    }

    public TimeValidationService Validate(TimeType timeType)
    {
        _validationContext = timeType;

        return this;
    }

    public TimeValidationService IsInOfficeHour()
    {
        _isValid = _validationContext >= _officeHour.StartTime && 
                   _validationContext <= _officeHour.EndTime;
        
        return this;
    }

    public TimeValidationService IsInSameTimeDuration(TimeType time)
    {
        _isValid = _validationContext.Time.Hour == time.Time.Hour;
        return this;
    }
    
    public TimeValidationService IsInBookingAvailableHour()
    {
        _isValid = _validationContext >= _bookingAvailableHour.StartTime && 
                   _validationContext <= _bookingAvailableHour.EndTime;
        
        return this;
    }

    public bool Result() => _isValid;

}