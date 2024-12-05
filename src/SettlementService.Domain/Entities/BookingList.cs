using System.Collections;

namespace SettlementService.Domain.Entities;

public class BookingList : IEnumerable<Booking>
{
    private ICollection<Booking> BookingCollection { get; set; } 

    public BookingList(IEnumerable<Booking> bookings) =>
       BookingCollection = bookings.ToList(); 
    public void Append(Booking booking)
    {
        if (BookingCollection == null)
            throw new ArgumentException("Booking can not be null", nameof(booking));
        
        BookingCollection.Append(booking);
    }
    
    public IEnumerator<Booking> GetEnumerator() => 
        BookingCollection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
