using MediatR;

namespace SettlementService.Application.Settlement.Commands.CreateSettlement;

public record CreateSettlementCommandResponse(Guid BookingId); 
public record CreateSettlementCommand(string BookingTime, string Name) : IRequest<CreateSettlementCommandResponse>;

public class CreateSettlementCommandHandler
    : IRequestHandler<CreateSettlementCommand, CreateSettlementCommandResponse>
{
    public async Task<CreateSettlementCommandResponse> Handle(CreateSettlementCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new CreateSettlementCommandResponse(Guid.CreateVersion7()));
    }
}