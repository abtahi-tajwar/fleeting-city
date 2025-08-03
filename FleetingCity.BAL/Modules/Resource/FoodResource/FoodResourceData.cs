using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Data;

public class FoodResourceData : BaseGameData<FoodResourceData, FoodResourceModel>
{
	protected override string ResourceFileName => "FoodResource";
	public FoodResourceData() : base() {}
}
