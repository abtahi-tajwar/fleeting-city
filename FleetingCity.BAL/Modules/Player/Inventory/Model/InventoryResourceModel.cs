using FleetingCity.BAL.Enum;

namespace FleetingCity.BAL.Model;

public class InventoryResourceModel
{
    public string Id { get; private set; }
    public string ResourceId { get; set; }
    public RESOURCE_TYPE Type { get; set; }
    public int Amount { get; set; }

    public InventoryResourceModel()
    {
        Id = Guid.NewGuid().ToString();
    }
}