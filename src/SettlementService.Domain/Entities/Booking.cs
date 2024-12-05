using SettlementService.Domain.ValueObjects;

namespace SettlementService.Domain.Entities;

[Serializable]
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
    public TimeType BookingTime { get; private set; }

    private Booking()
    {
        BookingId = BookingId.New();
    }

    public Booking(NameType name, TimeType bookingTime) : this() =>
        (Name, BookingTime) = (name, bookingTime);
}

