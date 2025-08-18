using System.Text.Json.Serialization;

namespace FleetingCity.BAL.Enum;

public enum RESOURCE_TYPE
{
    [JsonPropertyName("FOOD")]
    FOOD,
    [JsonPropertyName("NATURAL")]
    NATURAL,
    [JsonPropertyName("MATERIAL")]
    MATERIAL
}