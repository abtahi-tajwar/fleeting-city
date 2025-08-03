using System.Text.Json;
using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Data;

public class FoodResourceData
{
	public static FoodResourceData Instance { get; private set; } = null;
	public List<FoodResourceModel> DataList { get; private set; }
	public Dictionary<string, FoodResourceModel> Data { get; private set; }

	public FoodResourceData()
	{
		if (Instance != null)
		{
			throw new Exception("Failed to Instantiate Singleton class FoodResourceData. Reason: Already istantiated!");
		}
		Instance = this;
		LoadDataFromJSON();
	}

	private void LoadDataFromJSON()
	{
		string json = Helper.Helper.GetGameDataJSON("FoodResource");
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		List<FoodResourceModel> foodResourceList = JsonSerializer.Deserialize<List<FleetingCity.BAL.Model.FoodResourceModel>>(json, options);
		DataList = foodResourceList;
		Data = new Dictionary<string, FoodResourceModel>();
		foreach (FoodResourceModel resource in foodResourceList) {
			Data.Add(resource.Id, resource);
		}

	}
}
