using FleetingCity.BAL.Module;

namespace FleetingCity.BAL.Model;

public class AnimalFarmResourceModel : IProducedResourceModel
{
    // Properties
    public IResourceModel Resource { get; set; }
    public int Quantity { get; set; }
    public float ProductionRatePerMinute { get; set; }
    public int MaxCapacity { get; set; }
}