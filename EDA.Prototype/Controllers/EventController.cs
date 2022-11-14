using EDA.Prototype.Domain.DomainObjects;
using EDA.Prototype.Infrastructure.CQS.Interfaces;
using EDA.Prototype.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EDA.Prototype.Controllers;

[ApiController]
public class EventController : ControllerBase
{
    private readonly IRequestHandler<SendEventRequest, bool> _sendEventRequestHandler;

    public EventController(IRequestHandler<SendEventRequest, bool> sendEventRequestHandler)
    {
        _sendEventRequestHandler = sendEventRequestHandler;
    }

    [HttpPost("api/events")]
    [Consumes("application/json")]
    public async Task<ActionResult> SendEvent([FromBody] SendEventRequest request, CancellationToken cancellationToken)
    {
        await _sendEventRequestHandler.HandleAsync(request, cancellationToken);

        return Ok(request);
    }
}