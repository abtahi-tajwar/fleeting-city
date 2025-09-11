using System.Collections.Generic;
using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public class FoodResourceModel : IResourceModel
{
    // Properties
    public string Name { get; set; }
    public string Id { get; set; }
    public RESOURCE_TYPE Type { get; set; }
    public Dictionary<NUTRITION_TYPE, float> NutritionalComposition { get; set; }
}