using MediatR;
using Microsoft.EntityFrameworkCore;
using SettlementService.Application.Common.Interfaces;
using SettlementService.Application.Exceptions;
using SettlementService.Application.Extensions;
using SettlementService.Domain.Entities;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Application.Settlement.Commands.CreateSettlement;

public record CreateSettlementCommandResponse(string BookingId);

public record CreateSettlementCommand(string BookingTime, string Name) : IRequest<CreateSettlementCommandResponse>;

public class CreateSettlementCommandHandler(IApplicationDbContenxt dbContext)
    : IRequestHandler<CreateSettlementCommand, CreateSettlementCommandResponse>
{
    public async Task<CreateSettlementCommandResponse> Handle(CreateSettlementCommand request, CancellationToken cancellationToken)
    {
        var requestTime = MilitaryTime.Create(request.BookingTime);
        var settlementPool = dbContext.Bookings
                .AsNoTracking()
                .ToList()
                .ToSettlementPool();
            
        if (settlementPool.IsReservable())
        {
            throw new RequestConflictException(
                nameof(CreateSettlementCommand), 
                "Request Conflict",
                $"Exceeded maximum of settlement spot at: {requestTime.Value}");
        }

        var newBooking = new Booking(
            FullName.Create(request.Name), 
            new BookingTime(requestTime));

        dbContext.Bookings.Add(newBooking);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateSettlementCommandResponse(newBooking.BookingId.ToString());
    }
}