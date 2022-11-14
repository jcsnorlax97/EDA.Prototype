using System;
using EDA.Prototype.Domain.DomainObjects;

namespace EDA.Prototype.External.Azure.Commands;

public readonly record struct SendEventToEventHubCommand(IEnumerable<EventDO> EventDOs);