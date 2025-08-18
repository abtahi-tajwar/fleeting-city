using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Module;

namespace FleetingCity.BAL.Model;

public class TreeResourceModel : IProducedResourceModel
{
    public IResourceModel Resource { get; set; }
    public int Quantity { get; set; }
    public int MaxCapacity { get; set; }
    public bool IsChoppingRequired { get; set; }
    public Dictionary<TREE_STATE, float> ProductionPerState { get; set; }
}