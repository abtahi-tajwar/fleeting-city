using FleetingCity.BAL.Enum;

namespace FleetingCity.BAL.Model;

public class NaturalResourceModel : IResourceModel
{
    // Properties
    public string Name { get; set; }
    public string Id { get; set; } // Assuming Id is needed for IGameDataModel
    public RESOURCE_TYPE Type { get; set; } = RESOURCE_TYPE.NATURAL;
    public SUPPLY_TYPE SupplyType { get; set; } = SUPPLY_TYPE.RESOURCE;
}