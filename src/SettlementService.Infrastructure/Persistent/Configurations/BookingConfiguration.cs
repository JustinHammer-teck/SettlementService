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
                name => NameTypeToString(name), name => FullName.Create(name))
            .HasColumnType("varchar(50)")
            .IsRequired();
        
        builder.Property(b => b.BookingTime)
            .HasConversion(
                time => TimeTypeToString(time), time => StringToTimeType(time))
            .IsRequired();
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

    private string NameTypeToString(NameType nameType) =>
        nameType switch
        {
            FullName fullName => $"{fullName.FirstName} {fullName.LastName}",
            _ => throw new ArgumentException($"Unsupported Name Type of type: {nameType}")
        };
}