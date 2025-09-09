using FleetingCity.BAL.Enum;

namespace FleetingCity.BAL.Model;

public class InventoryConsumableModel
{
    public string Id { get; private set; }
    public string ResourceId { get; set; }
    public CONSUMABLE_TYPE Type { get; set; }
    public int Amount { get; set; }
}