using MediatR;
using Microsoft.AspNetCore.Mvc;
using SettlementService.Application.Settlement.Commands.CreateSettlement;

namespace SettlementService.Api.Controllers;

public class SettlementController : ApiControllerBase
{
    public SettlementController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    public IResult Get(int id)
    {
        return Results.Ok("success");
    }
    
    [HttpPost]
    public IResult BookingSettlement([FromBody] CreateSettlementCommand command)
    {
        return Results.Created();
    }
}