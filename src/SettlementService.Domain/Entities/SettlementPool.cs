using System.Collections;
using SettlementService.Domain.Constants;

namespace SettlementService.Domain.Entities;

public class SettlementPool : IEnumerable<Booking>
{
    private int Max => SettlementOptions.MaxSpotHeld;
    public List<Booking> BookingCollection { get; set; }

    public SettlementPool(IEnumerable<Booking> bookings)
    {
        BookingCollection = bookings.ToList();
    }

    public bool IsReservable()
        => BookingCollection.Count >= Max;

    public IEnumerator<Booking> GetEnumerator() => BookingCollection.GetEnumerator(); 
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}