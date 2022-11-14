using System;
using LanguageExt;
using LanguageExt.Common;

namespace EDA.Prototype.Domain.DomainObjects;

public record EventMessageDO
{
    private readonly string _value;

    private EventMessageDO(string value)
    {
        _value = value;
    }

    public static Option<EventMessageDO> Create(string message)
    {
        return Option<EventMessageDO>.Some(new EventMessageDO(message));
    }

    public static implicit operator string(EventMessageDO eventMessage) => eventMessage._value;
}