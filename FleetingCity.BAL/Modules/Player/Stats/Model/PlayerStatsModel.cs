using System.Collections.Generic;
using FleetingCity.BAL.Enum;

namespace FleetingCity.BAL.Model;

public class PlayerStatsModel
{
    public Dictionary<NUTRITION_TYPE, float> Nutrition { get; set; } = new Dictionary<NUTRITION_TYPE, float>
    {
        { NUTRITION_TYPE.PROTEIN, 0f },
        { NUTRITION_TYPE.CARBS, 0f },
        { NUTRITION_TYPE.VITAMIN, 0f }
    };
}