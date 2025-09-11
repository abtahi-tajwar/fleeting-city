using FleetingCity.BAL.Data;
using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Service;

public class ResourceService
{
    public static IResourceModel GetResource(string resourceId)
	{
		var foodResourceData = FoodResourceData.Instance.Data;
		var naturalResourceData = NaturalResourceData.Instance.Data;
		var materialResourceData = MaterialResourceData.Instance.Data;

		if (foodResourceData.TryGetValue(resourceId, out var foodResource))
		{
			return foodResource;
		} else if (naturalResourceData.TryGetValue(resourceId, out var naturalResource))
		{
			return naturalResource;
		}
		else if (materialResourceData.TryGetValue(resourceId, out var materialResource))
		{
			return materialResource;
		}
		else
		{
			throw new KeyNotFoundException($"Resource with ID '{resourceId}' not found.");
		}
	}
}