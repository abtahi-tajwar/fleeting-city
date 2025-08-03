using System.Data.Common;

namespace FleetingCity.BAL.Model;

public class AnimalFarmUnitModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float ProductionRatePerMinute { get; set; }
}