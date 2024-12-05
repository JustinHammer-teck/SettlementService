using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SettlementService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
   public IMediator _mediator;
}