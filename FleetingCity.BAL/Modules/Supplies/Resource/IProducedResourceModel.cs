using FleetingCity.BAL.Helper;
using FleetingCity.BAL.Model;
using FleetingCity.BAL.Service;

namespace FleetingCity.BAL.Module;

public abstract class IProducedResourceModel
{
    public string ResourceId { get; set; }
    // public int Quantity { get; set; }
    public int MaxCapacity { get; set; }

    public IResourceModel GetResource()
    {
        return ResourceService.GetResource(ResourceId);
    }
}