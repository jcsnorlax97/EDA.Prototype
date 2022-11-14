using System.Linq;
using EDA.Prototype.Domain.DomainObjects;
using EDA.Prototype.External.Azure.Commands;
using EDA.Prototype.Infrastructure.CQS.Interfaces;
using EDA.Prototype.Requests;
using LanguageExt;

namespace EDA.Prototype.RequestHandlers;

// TODO: Make the generic types for IRequestHandler functional, by using Result, Option, Union, and/or Unit.
public class SendEventRequestHandler : IRequestHandler<SendEventRequest, bool>
{
	private readonly ICommandHandler<SendEventToEventHubCommand> _sendEventToEventHubCommandHandler;

	public SendEventRequestHandler(ICommandHandler<SendEventToEventHubCommand> sendEventToEventHubCommandHandler)
	{
		_sendEventToEventHubCommandHandler = sendEventToEventHubCommandHandler;
    }

	public async Task<bool> HandleAsync(SendEventRequest request, CancellationToken cancellationToken)
	{
		// TODO: Convert Request (DTO) into a Domain Object. (Done, but I think there exists improvements here as well.)
		// TODO: I think there should be ways to improve the validation logic, instead of doing `Count()` comparison.
		IEnumerable<Option<EventMessageDO>> optionalEventMessageDOs = request.EventContent.Select(eventContent => EventMessageDO.Create(eventContent));
		IEnumerable<EventMessageDO> eventMessageDOs = optionalEventMessageDOs.Somes<EventMessageDO>();
        if (optionalEventMessageDOs.Count() != eventMessageDOs.Count()) return false;

        IEnumerable<EventDO> eventDOs = eventMessageDOs.Select(eventMessageDO => new EventDO(eventMessageDO));

		// send event to event hub (event hub client)
		await _sendEventToEventHubCommandHandler.HandleAsync(new SendEventToEventHubCommand(eventDOs), cancellationToken);

		return true;
	}
}