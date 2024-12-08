using Microsoft.EntityFrameworkCore;
using SettlementService.Application.Common.Interfaces;
using SettlementService.Domain.Entities;

namespace SettlementService.Infrastructure.Persistent;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContenxt
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}