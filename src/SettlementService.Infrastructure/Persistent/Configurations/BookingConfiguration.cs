using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SettlementService.Domain.Entities;
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
                name => NameTypeToString(name), name => StringToNameType(name))
            .HasColumnType("varchar(50)");
        
        builder.Property(b => b.BookingTime)
            .HasConversion(time => TimeTypeToString(time), time => StringToTimeType(time)
            ).IsRequired();
    }
    
    private string TimeTypeToString(TimeType timeType) =>
        timeType switch
        {
            MilitaryTime militaryTime => militaryTime.ToString(),
            _ => throw new ArgumentException($"Unsupported TimeType: {timeType.GetType().Name}")
        };

    private TimeType StringToTimeType(string time)
    {
        var formats = new Dictionary<string, Func<string, TimeType>>
        {
            {"HH:mm" , MilitaryTime.Create}
        };

        var result = formats
            .Where(strategy => 
                TimeOnly.TryParseExact(time, strategy.Key, out _))
            .Select(x => 
                x.Value(time))
            .FirstOrDefault();
        
        return result ?? throw new ArgumentException($"Unsupported time format for time of value : {time}"); 
    }

    private NameType StringToNameType(string fullName) =>
        fullName.Split(" ") switch
        {
            [var firstName, var lastName] => FullName.Create(firstName, lastName),
            [var name] => MononymName.Create(name), 
            _ => throw new ArgumentException($"Invalid Format of full name")
        };

    private string NameTypeToString(NameType nameType) =>
        nameType switch
        {
            FullName fullName => $"{fullName.FirstName} {fullName.LastName}",
            MononymName name => $"{name}",
            _ => throw new ArgumentException($"Unsupported Name Type of type: {nameType}")
        };
}