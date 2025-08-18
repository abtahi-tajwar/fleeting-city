using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Module;

public interface IProducedResourceModel
{
    public IResourceModel Resource { get; set; }
    public int Quantity { get; set; }
    public int MaxCapacity { get; set; }
}