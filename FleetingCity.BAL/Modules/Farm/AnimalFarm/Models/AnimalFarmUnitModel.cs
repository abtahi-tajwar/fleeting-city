using System.Data.Common;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public class AnimalFarmUnitModel : IGameDataModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float ProductionRatePerMinute { get; set; }
}