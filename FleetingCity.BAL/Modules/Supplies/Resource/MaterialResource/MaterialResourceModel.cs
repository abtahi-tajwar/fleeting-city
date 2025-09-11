using FleetingCity.BAL.Enum;

namespace FleetingCity.BAL.Model;

public class MaterialResourceModel : IResourceModel
{
    // Properties
    public string Name { get; set; }
    public string Id { get; set; } // Assuming Id is needed for IGameDataModel
    public RESOURCE_TYPE Type { get; set; }
    // Composition : <resourceId, proportion>
    public Dictionary<string, float> Composition { get; set; }
}   