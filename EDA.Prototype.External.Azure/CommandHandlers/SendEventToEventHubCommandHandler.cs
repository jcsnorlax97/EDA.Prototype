using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EDA.Prototype.External.Azure.Commands;
using EDA.Prototype.Infrastructure.CQS.Interfaces;
using Newtonsoft.Json;

namespace EDA.Prototype.External.Azure.CommandHandlers;

public class SendEventToEventHubCommandHandler : ICommandHandler<SendEventToEventHubCommand>
{
	private readonly EventHubProducerClient _eventHubProducerClient;

	public SendEventToEventHubCommandHandler(EventHubProducerClient eventHubProducerClient)
	{
		_eventHubProducerClient = eventHubProducerClient;
	}

	public async Task HandleAsync(SendEventToEventHubCommand command, CancellationToken cancellationToken)
	{
		using EventDataBatch eventBatch = await _eventHubProducerClient.CreateBatchAsync();

		// TODO (Refactoring): Source this out into its own method instead. 
		// TODO (Refactoring): Use SendEventToEventHubCommand to determine "numOfEvents" instead.
		foreach (var eventDO in command.EventDOs)
		{
            if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventDO)))))
			{
                throw new Exception($"Event {eventDO.EventMessageDO} is too large for the batch and cannot be sent.");
            }

        }

		// Use the producer client to send the batch of events to the event hub
		await _eventHubProducerClient.SendAsync(eventBatch);
		Console.WriteLine($"A batch of {command.EventDOs.Count()} events has been published.");
    }
}