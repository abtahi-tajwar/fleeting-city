using System.Collections.Generic;
using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public class FoodResourceModel : IGameDataModel
{
    // Properties
    public string Name { get; set; }
    public string Id { get; set; }
    public Dictionary<NUTRITION_TYPE, float> NutritionalComposition { get; set; }
}