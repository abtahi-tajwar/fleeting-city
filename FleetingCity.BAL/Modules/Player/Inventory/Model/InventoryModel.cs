using FleetingCity.BAL.Dto;
using FleetingCity.BAL.Service;

namespace FleetingCity.BAL.Model;

public class InventoryModel
{
	// Dictionary <Resourceid, InventoryResourceModel> 
	public Dictionary<string, InventoryResourceModel> Resources { get; set; } = new();
	// Dictionary <ConsumableId, InventoryResourceModel> 
	public Dictionary<string, InventoryConsumableModel> Consumable { get; set; } = new();
	// Dictionary <ToolId, InventoryResourceModel> 
	public List<InventoryToolModel> Tools { get; set; } = new();

	public void AddResource(InventoryModelAddResourceDto r)
{
	var resource = ResourceService.GetResource(r.ResourceId);

	if (Resources.TryGetValue(r.ResourceId, out var res))
	{
		res.Amount += r.Amount;
	}
	else
	{
		var newResource = new InventoryResourceModel
		{
			Amount = r.Amount,
			Type = resource.Type,
			ResourceId = r.ResourceId
		};

		Resources[r.ResourceId] = newResource; // ✅ add to dictionary
	}
}

}
