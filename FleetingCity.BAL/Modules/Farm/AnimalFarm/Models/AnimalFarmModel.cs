using System;
using System.Collections.Generic;
using FleetingCity.BAL.Enum;

namespace  FleetingCity.BAL.Model;

public class AnimalFarmModel
{
    // Properties
    /**
    → CurrentState
→ ProducedResource : AnimalFarmResource
→ Unit: AnimalFarmUnit
→ ResourcePerUnit : Dictionary<Resource, float>
→ MinimumUnitThreshold: int
→ LastFed: DateTime
**/
    public int CurrentUnits { get; set; }
    public int MaximumUnits { get; set; }
    public ANIMAL_FARM_STATES CurrentState { get; set; }
    public List<AnimalFarmResourceModel> ProducedResource { get; set; }
    public AnimalFarmUnitModel Unit { get; set; }
    public Dictionary<string, float> ResourcePerUnit { get; set; }
    public int MinimumUnitThreshold { get; set; }
    public DateTime LastFed { get; set; }

}


