using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SettlementService.Domain.Entities;
using SettlementService.Domain.Exceptions;
using SettlementService.Domain.ValueObjects;

namespace SettlementService.Infrastructure.Persistent.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Booking");

        builder.HasKey(b => b.BookingId);

        builder.Property(b => b.BookingId)
            .IsRequired()
            .HasConversion(
                guid => guid.Value, guid => new BookingId(guid));

        builder.Property(s => s.Name)
            .HasConversion(
                name => NameTypeToString(name), name => FullName.Create(name))
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.ComplexProperty(s => s.BookingTime, builder =>
        {
            builder.Property(x => x.Time)
                .HasConversion(time => TimeTypeToString(time), time => StringToTimeType(time))
                .IsRequired();

            builder.Property(x => x.Hour)
                .HasConversion(hour => hour.ToString(), hour => int.Parse((string)hour))
                .IsRequired();
        });
    }

    private string TimeTypeToString(TimeType timeType)
    {
        return timeType switch
        {
            MilitaryTime militaryTime => militaryTime.ToString(),
            _ => throw new UnsupportedTimeTypeException($"Invalid TimeType with type: {timeType.GetType()}")
        };
    }

    private TimeType StringToTimeType(string time)
    {
        /*
         * I Can create a list of Func like this from Assembly to follow Open/Close principle
         * but this implementation is good enough for this demo
         */
        var formatStrategies = new Dictionary<string, Func<string, TimeType>>
        {
            { MilitaryTime.DefaultFormat, MilitaryTime.Create }
        };

        foreach (var (format, cast) in formatStrategies)
            if (TimeOnly.TryParseExact(time, format, out _))
                return cast(time);

        throw new UnsupportedTimeTypeException($"Unsupported time format for time of value : {time}");
    }

    private string NameTypeToString(NameType nameType)
    {
        return nameType switch
        {
            FullName fullName => $"{fullName.FirstName} {fullName.LastName}",
            _ => throw new UnsupportedNameTypeException($"Unsupported Name Type of type: {nameType}")
        };
    }
}