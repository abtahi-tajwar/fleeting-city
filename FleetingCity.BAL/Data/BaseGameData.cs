using System.ComponentModel;
using System.Text.Json;
using FleetingCity.BAL.Provider;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Data;

public abstract class BaseGameData<T, TModel> : IGameDataMarker
	where T : BaseGameData<T, TModel>
	where TModel : IGameDataModel
{
	public static T Instance { get; private set; } = null;
	protected abstract string ResourceFileName { get; }

	public List<TModel> DataList { get; private set; }
	public Dictionary<string, TModel> Data { get; private set; }

	protected BaseGameData()
	{
		if (Instance != null) throw new Exception($"Failed to Instantiate Singleton class BaseGameData. Reason: Already istantiated!");
		Instance = (T)this;
		LoadDataFromJSON();
	}

	protected void LoadDataFromJSON() {
		string json = Helper.Helper.GetGameDataJSON("FoodResource");
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		List<TModel> foodResourceList = JsonSerializer.Deserialize<List<TModel>>(json, options);
		DataList = foodResourceList;
		Data = new Dictionary<string, TModel>();
		foreach (TModel resource in foodResourceList) {
			Data.Add(resource.Id, resource);
		}
	}
}


public interface IGameDataMarker { }
