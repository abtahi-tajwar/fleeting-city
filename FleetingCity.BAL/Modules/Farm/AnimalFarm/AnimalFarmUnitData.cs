using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Data;

public class AnimalFarmUnitData : BaseGameData<AnimalFarmUnitData, AnimalFarmUnitModel>
{
    protected override string ResourceFileName => "AnimalFarmUnit";

    public AnimalFarmUnitData() : base () {}
}