namespace SettlementService.Domain.Constants;

public static class SettlementOptions
{
    public const int MaxSpotHeld = 4;
    public static readonly TimeSpan ReserveDuration = TimeSpan.FromHours(1);
}