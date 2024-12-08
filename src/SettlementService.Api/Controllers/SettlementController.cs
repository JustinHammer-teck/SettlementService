using Microsoft.AspNetCore.Mvc;
using SettlementService.Application.Settlement.Commands.CreateSettlement;

namespace SettlementService.Api.Controllers;

public class SettlementController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateSettlementCommandResponse>>
        BookingSettlement(CreateSettlementCommand command)
    {
        return await Mediator.Send(command);
    }
}