using System.Text.Json.Serialization;
using Godot;

public enum INVENTORY_SCREEN
{
    [JsonPropertyName("ITEMS")]
    ITEMS,
    [JsonPropertyName("RESOURCES")]
    RESOURCES
}