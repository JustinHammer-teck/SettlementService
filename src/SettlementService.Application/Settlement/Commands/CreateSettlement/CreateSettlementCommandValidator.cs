using FluentValidation;
using SettlementService.Domain.Entities;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Application.Settlement.Commands.CreateSettlement;

public class CreateSettlementCommandValidator : AbstractValidator<CreateSettlementCommand>
{
    public CreateSettlementCommandValidator(
        OfficeHour officeHour, BookingAvailableHour bookingAvailableHour)
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(50)
            .Custom((requestName, context) =>
            {
                if (!FullName.TryCreate(requestName, out _))
                    context.AddFailure("Name", "Invalid name format");
            });

        RuleFor(v => v.BookingTime)
            .NotEmpty()
            .Custom((requestTime, context) =>
            {
                var time = MilitaryTime.Create(requestTime);
                var isInOfficeHour = time >= officeHour.StartTime &&
                                     time <= officeHour.EndTime;
                if (!isInOfficeHour)
                    context.AddFailure("BookingTime",
                        $"Booking time must be within business hours: {officeHour.StartTime} to {officeHour.EndTime}.");

                var isInBookingAvailableHour = time >= bookingAvailableHour.StartTime &&
                                               time <= bookingAvailableHour.EndTime;
                if (!isInBookingAvailableHour)
                    context.AddFailure("BookingTime",
                        $"Latest booking start time is {bookingAvailableHour.EndTime} to ensure completion by {officeHour.EndTime}.");
            });
    }
}