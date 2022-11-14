using LanguageExt;
using Newtonsoft.Json;

namespace EDA.Prototype.Domain.DomainObjects;

public record EventDO
{
    public EventMessageDO EventMessageDO { get; private set; }

    public EventDO(EventMessageDO eventMessageDO)
    {
        EventMessageDO = eventMessageDO;
    }
}