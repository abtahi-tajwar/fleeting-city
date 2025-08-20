using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Module;

namespace FleetingCity.BAL.Model;

public class TreeResourceModel : IProducedResourceModel
{
    public bool IsChoppingRequired { get; set; }
    public bool IsStumpingRequired { get; set; }
    public float? ProductionIntervalInMinutes { get; set; }
    public Dictionary<TREE_STATE, float> ProductionPerState { get; set; }
}