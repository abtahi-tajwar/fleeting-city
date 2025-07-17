using Godot;
using System;

public partial class UnitSelectionManager : Node
{
    // Statics
    public static bool IsPlayerSelected { get; set; } = false;
    public static string? SelectedSettlerId { get; set; } = null;

    // Methods
    public static void SelectUnit(Node unit)
    {
        if (unit is Player player)
        {
            IsPlayerSelected = true;
            SelectedSettlerId = null; // Clear any selected settler ID
            GD.Print("Player unit selected.");
        }
        else if (unit is Settler settler)
        {
            IsPlayerSelected = false;
            SelectedSettlerId = settler.UniqueId;
            GD.Print($"Settler unit selected with UniqueId: {settler.UniqueId}");
        }
        else
        {
            IsPlayerSelected = false;
            SelectedSettlerId = null;
            GD.Print("Unit selection failed. Not a valid unit type.");
        }
    }
}
