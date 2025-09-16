using System.Collections.Generic;
using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public class FoodResourceModel : IResourceModel
{
    // Properties
    public string Name { get; set; }
    public string Id { get; set; }
    public RESOURCE_TYPE Type { get; set; } = RESOURCE_TYPE.FOOD;
    public SUPPLY_TYPE SupplyType { get; set; } = SUPPLY_TYPE.RESOURCE;
    public Dictionary<NUTRITION_TYPE, float> NutritionalComposition { get; set; }
}