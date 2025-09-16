using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public interface ISupplyModel : IGameDataModel
{
    // Properties
    public string Name { get; set; }
    public SUPPLY_TYPE SupplyType { get; set; }
}