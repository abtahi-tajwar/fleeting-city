using System.Collections.Generic;

namespace FleetingCity.BAL;

public class FoodResourceModel
{
    // Properties
    public string Name { get; set; }
    public string Id { get; set; }
    public Dictionary<NUTRITION_TYPE, float> NutritionalComposition { get; set; }
}