using EDA.Prototype.Domain.DomainObjects;
using Newtonsoft.Json;

namespace EDA.Prototype.Requests;

public record SendEventRequest
{
    [JsonProperty(PropertyName = "eventContent")]
    public IEnumerable<string> EventContent { get; init; } = Enumerable.Empty<string>();
}