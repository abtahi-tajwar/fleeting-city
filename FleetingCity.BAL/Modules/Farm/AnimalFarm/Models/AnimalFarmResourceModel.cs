namespace FleetingCity.BAL.Model;

public class AnimalFarmResourceModel
{
    // Properties
    public FoodResourceModel Resource { get; set; }
    public int Quantity { get; set; }
    public float ProductionRatePerMinute { get; set; }
    public int MaxCapacity { get; set; }
}