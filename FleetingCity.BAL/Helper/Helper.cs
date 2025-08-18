using System.Reflection;
using System.Text.Json;
using FleetingCity.BAL.Data;
using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Helper;

public class Helper
{
	public static string GetGameDataJSON(string resourceFileName)
	{
		var path = Path.Combine(AppContext.BaseDirectory, "Data", $"{resourceFileName}.json");
		string json = File.ReadAllText(path);
		return json;
	}

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
