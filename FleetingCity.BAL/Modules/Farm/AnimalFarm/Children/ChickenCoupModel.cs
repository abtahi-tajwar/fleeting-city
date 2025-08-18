using FleetingCity.BAL.Data;
using FleetingCity.BAL.Enum;

namespace FleetingCity.BAL.Model;

public class ChickenCoupModel : AnimalFarmModel
{
	public ChickenCoupModel()
	{
		base.CurrentUnits = 0;
		base.MaximumUnits = 15;
		base.MinimumUnitThreshold = 2;
		base.CurrentState = ANIMAL_FARM_STATES.UNFED;
		base.ProducedResource = new()
		{
			new() {
				ResourceId = "egg",
				Quantity = 0,
				ProductionRatePerMinute = 1,
				MaxCapacity = 30
			},
			new() {
				ResourceId = "chicken_meat",
				Quantity = 0,
				ProductionRatePerMinute = 5,
				MaxCapacity = 20
			},
		};
		base.ResourcePerUnit = new()
		{
			{ "chicken_meat", 1f },
			{ "egg", 5f }
		};
	}
}
