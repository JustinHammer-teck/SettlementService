using Microsoft.EntityFrameworkCore;
using SettlementService.Domain.Entities;

namespace SettlementService.Application.Common.Interfaces;

public interface IApplicationDbContenxt
{
    DbSet<Booking> Bookings { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}