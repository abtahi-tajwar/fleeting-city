using FleetingCity.BAL.Data;
using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public class TreeStateModel
{
    public TREE_STATE State { get; set; }
    public float GrowingDurationInMinutes { get; set; }
}

public class TreeDataModel : IGameDataModel
{
    /// <summary>
    /// Unique identifier for this tree (for saving/lookup).
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The type/category of tree (Normal, Pine, Oak, etc).
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The resource this tree yields (e.g. Wood, Sap, etc).
    /// </summary>
    public List<TreeResourceModel> ProducedResource { get; set; }

    /// <summary>
    /// List of states in which this tree can be harvested.
    /// </summary>
    public List<TREE_STATE> HarvestableStates { get; set; } = new();

    /// <summary
    /// List of states and their growing durations.
    /// </summary
    public List<TreeStateModel> States { get; set; } = new();
}