using Microsoft.EntityFrameworkCore;
using SettlementService.Application.Common.Interfaces;
using SettlementService.Domain.Entities;

namespace SettlementService.Infrastructure.Persistent;

internal sealed class ApplicationDbContext : DbContext, IApplicationDbContenxt
{
    public DbSet<Booking> Bookings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}