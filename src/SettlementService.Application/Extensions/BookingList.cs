using SettlementService.Domain.Entities;

namespace SettlementService.Application.Extensions;

public static class BookingList
{
    public static SettlementPool ToSettlementPool(this IEnumerable<Booking> bookings)
        => new (bookings);
}
