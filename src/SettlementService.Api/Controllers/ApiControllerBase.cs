using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SettlementService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
   public ISender _mediator;
   
   protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}