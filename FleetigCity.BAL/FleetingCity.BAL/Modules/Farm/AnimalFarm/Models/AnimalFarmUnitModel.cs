using System.Data.Common;

namespace FleetingCity.BAL;

public class AnimalFarmUnitModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float ProductionRatePerMinute { get; set; }
}