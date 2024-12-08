using SettlementService.Domain.ValueObjects;

namespace SettlementService.Domain.Entities;


/*
 * We tense to be primitive obsession with our Model design
 * by enforcing strictly type we can reduce bug in our application.
 */
public readonly record struct BookingId(Guid Value)
{
    public static BookingId Empty() => new(Guid.Empty);
    public static BookingId New() => new(Guid.CreateVersion7());
    public override string ToString() => Value.ToString();
}

public class Booking
{
    public BookingId BookingId { get; private set; }
    public NameType Name { get; private set; }
    public BookingTime BookingTime { get; private set; }

    private Booking()
    {
        BookingId = BookingId.New();
    }

    public Booking(NameType name, BookingTime bookingTime) : this() =>
        (Name, BookingTime) = (name, bookingTime);
}

