using System.Reflection;
using System.Text.Json;

namespace FleetingCity.BAL.Helper;

public class Helper
{
	public static string GetGameDataJSON(string resourceFileName)
	{
		var path = Path.Combine(AppContext.BaseDirectory, "Data", $"{resourceFileName}.json");
		string json = File.ReadAllText(path);
		return json;
	}
}
