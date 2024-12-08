using System.Collections;
using SettlementService.Domain.Constants;

namespace SettlementService.Domain.Entities;

public class SettlementPool : IEnumerable<Booking>
{
    public SettlementPool(IEnumerable<Booking> bookings)
    {
        BookingCollection = bookings.ToList();
    }

    private int Max => SettlementOptions.MaxSpotHeld;
    public List<Booking> BookingCollection { get; set; }

    public IEnumerator<Booking> GetEnumerator()
    {
        return BookingCollection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool IsReservable()
    {
        return BookingCollection.Count >= Max;
    }
}