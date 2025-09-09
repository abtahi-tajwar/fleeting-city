using FleetingCity.BAL.Enum;

namespace FleetingCity.BAL.Model;

public class InventoryToolModel
{
    public string Id { get; private set; }
    public string ResourceId { get; set; }
    public TOOL_TYPE Type { get; set; }
    public int Amount { get; set; }
}