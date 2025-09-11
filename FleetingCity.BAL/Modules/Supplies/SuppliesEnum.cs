using System.Text.Json.Serialization;

namespace FleetingCity.BAL.Enum;

public enum SUPPLY_TYPE
{
    [JsonPropertyName("RESOURCE")]
    RESOURCE,
    [JsonPropertyName("CONSUMABLE")]
    CONSUMABLE,
    [JsonPropertyName("TOOL")]
    TOOL
}

public enum RESOURCE_TYPE
{
    [JsonPropertyName("FOOD")]
    FOOD,
    [JsonPropertyName("NATURAL")]
    NATURAL,
    [JsonPropertyName("MATERIAL")]
    MATERIAL
}

public enum CONSUMABLE_TYPE
{
    [JsonPropertyName("EDIBLE")]
    EDIBLE,
    [JsonPropertyName("POTION")]
    POTION
}

public enum TOOL_TYPE
{
    [JsonPropertyName("WEAPON")]
    WEAPON,
    [JsonPropertyName("FARMING")]
    FARMING
}