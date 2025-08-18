using FleetingCity.BAL.Enum;
using FleetingCity.BAL.Utils;

namespace FleetingCity.BAL.Model;

public class TreeModel : IGameDataModel
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
    /// The current lifecycle state of the tree.
    /// </summary>
    public TREE_STATE CurrentState { get; set; }

    /// <summary>
    /// True if the tree is currently in a chopping animation/action.
    /// </summary>
    public bool IsBeingChopped { get; set; }

    /// <summary>
    /// The resource this tree yields (e.g. Wood, Sap, etc).
    /// </summary>
    public List<TreeResourceModel> ProducedResource { get; set; }

    /// <summary>
    /// List of states in which this tree can be harvested.
    /// </summary>
    public List<TREE_STATE> HarvestableStates { get; set; } = new();


    public TreeModel()
    {
        
    }

}