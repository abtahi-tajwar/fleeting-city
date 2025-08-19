using FleetingCity.BAL.Helper;
using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Module;

public abstract class IProducedResourceModel
{
    public string ResourceId { get; set; }
    // public int Quantity { get; set; }
    public int MaxCapacity { get; set; }

    public IResourceModel GetResource()
    {
        return Helper.Helper.GetResource(ResourceId);
    }
}