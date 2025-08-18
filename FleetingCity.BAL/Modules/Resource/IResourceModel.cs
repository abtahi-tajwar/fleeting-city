using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public interface IResourceModel : IGameDataModel
{
    string Name { get; set; }
    RESOURCE_TYPE Type { get; set; }
}