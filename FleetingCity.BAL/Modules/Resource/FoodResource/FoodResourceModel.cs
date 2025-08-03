using System.Collections.Generic;
using FleetingCity.BAL.Enum;

namespace FleetingCity.BAL.Model;

public class FoodResourceModel
{
    // Properties
    public string Name { get; set; }
    public string Id { get; set; }
    public Dictionary<NUTRITION_TYPE, float> NutritionalComposition { get; set; }
}