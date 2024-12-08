using FluentAssertions;
using SettlementService.Application.Exceptions;
using SettlementService.Application.Settlement.Commands.CreateSettlement;
using SettlementService.Domain.Entities;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Application.Test.Settlement;

[TestFixture]
public class CreateSettlementCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateSettlement_WhenPoolIsReservable()
    {
        // Arrange
        var existingBooking = new[]
        {
            new Booking(FullName.Create("John Doe"), new BookingTime(MilitaryTime.Create("09:00"))),
            new Booking(FullName.Create("John Doe2"), new BookingTime(MilitaryTime.Create("09:30"))),
            new Booking(FullName.Create("John Doe3"), new BookingTime(MilitaryTime.Create("10:30")))
        };

        foreach (var booking in existingBooking) await Test.AddAsync(booking);

        var command = new CreateSettlementCommand("10:00", "Alice Johnson");

        // Act
        var response = await Test.SendAsync(command);

        // Assert
        response.BookingId.Should().NotBeNullOrEmpty();

        var bookingId = new BookingId(Guid.Parse(response.BookingId));
        var createdBooking = await Test.FindAsync<Booking>(bookingId);

        createdBooking.Should().NotBeNull();
        createdBooking.Name.Should().Be(FullName.Create("Alice Johnson"));
        createdBooking.BookingTime.Time.Should().Be(MilitaryTime.Create("10:00"));
    }

    [Test]
    public async Task ShouldThrowRequestConflictException_WhenPoolIsNotReservable()
    {
        // Arrange
        var bookings = new[]
        {
            new Booking(FullName.Create("John Doe"), new BookingTime(MilitaryTime.Create("09:00"))),
            new Booking(FullName.Create("Jane Smith"), new BookingTime(MilitaryTime.Create("09:30"))),
            new Booking(FullName.Create("Jane Smith2"), new BookingTime(MilitaryTime.Create("10:30"))),
            new Booking(FullName.Create("Jane Smith3"), new BookingTime(MilitaryTime.Create("10:45")))
        };

        foreach (var booking in bookings) await Test.AddAsync(booking);

        var command = new CreateSettlementCommand("09:30", "Bob Johnson");

        // Act
        Func<Task> act = async () => await Test.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<RequestConflictException>()
            .WithMessage("*Exceeded maximum of settlement spot*");
    }

    [Test]
    public async Task ShouldAddBookingToDatabase()
    {
        // Arrange
        var command = new CreateSettlementCommand("12:00", "Eve Black");
        // Act
        var response = await Test.SendAsync(command);
        var bookingId = new BookingId(Guid.Parse(response.BookingId));
        // Assert
        var createdBooking = await Test.FindAsync<Booking>(bookingId);
        createdBooking.Should().NotBeNull();
        createdBooking.Name.Should().Be(FullName.Create("Eve Black"));
        createdBooking.BookingTime.Time.Should().Be(MilitaryTime.Create("12:00"));
    }
}