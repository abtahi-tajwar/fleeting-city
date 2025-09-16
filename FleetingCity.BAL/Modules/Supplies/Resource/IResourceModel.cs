using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public interface IResourceModel : ISupplyModel
{
    RESOURCE_TYPE Type { get; set; }
}