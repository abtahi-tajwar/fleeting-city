using System.Text.Json;

namespace FleetingCity.BAL.Data;

public class FoodResourceData
{
    public FoodResourceData Instance = null;
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

        List<FoodResourceModel> foodResourceList = JsonSerializer.Deserialize<List<FoodResourceModel>>(json, options);
        DataList = foodResourceList;

        foreach (FoodResourceModel resource in foodResourceList) {
            Data.Add(resource.Id, resource);
        }

    }
}