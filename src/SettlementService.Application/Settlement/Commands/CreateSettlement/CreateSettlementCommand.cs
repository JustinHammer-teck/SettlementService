using MediatR;
using Microsoft.EntityFrameworkCore;
using SettlementService.Application.Common.Interfaces;
using SettlementService.Application.Exceptions;
using SettlementService.Application.Services.Time;
using SettlementService.Domain.Constants;
using SettlementService.Domain.Entities;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Application.Settlement.Commands.CreateSettlement;

public record CreateSettlementCommandResponse(string BookingId);

public record CreateSettlementCommand(string BookingTime, string Name) : IRequest<CreateSettlementCommandResponse>;

public class CreateSettlementCommandHandler(
    TimeValidationService timeValidationService, 
    IApplicationDbContenxt dbContext)
    : IRequestHandler<CreateSettlementCommand, CreateSettlementCommandResponse>
{
    public async Task<CreateSettlementCommandResponse> Handle(CreateSettlementCommand request, CancellationToken cancellationToken)
    {
        var bookings = await dbContext.Bookings
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
            
        var count = bookings.Count(booking =>
                timeValidationService
                    .Validate(booking.BookingTime)
                    .IsInOfficeHour()
                    .IsInSameTimeDuration(MilitaryTime.Create(request.BookingTime))
                    .IsInBookingAvailableHour()
                    .Result());

        if (count > SettlementOptions.MaxSpotHeld)
            throw new RequestConflictException(
                nameof(CreateSettlementCommand), 
                "Request Conflict", 
                "Exceeded maximum of settlement spot");
        
        var newBooking = new Booking(
            FullName.Create(request.Name), 
            MilitaryTime.Create(request.BookingTime));

        dbContext.Bookings.Add(newBooking);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateSettlementCommandResponse(newBooking.BookingId.ToString());
    }
}