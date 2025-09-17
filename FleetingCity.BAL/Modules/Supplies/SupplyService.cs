using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Service;

public class SupplyService
{
    public static ISupplyModel GetById(string id)
    {
        return ResourceService.GetResource(id);
    }
}