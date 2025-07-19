using Godot;
using System;

public partial class Settler : CharacterBody2D
{
    public string UniqueId { get; set; }

    // Services
    private CharacterMovementService _movementService;

    public override void _Ready()
    {
        UniqueId = Guid.NewGuid().ToString(); // Assign a unique ID to the settler
        GD.Print($"Settler created with UniqueId: {UniqueId}");
        _movementService = new CharacterMovementService(this);
        UnitSelectionManager.SetOutline(this, false);
    }
    public override void _PhysicsProcess(double delta)
    {
        _movementService.Update(delta);
    }
    public override void _Process(double delta)
    {
        // Additional processing logic can be added here if needed
        if (UnitSelectionManager.SelectedSettlerId == UniqueId)
        {
            GD.Print($"Processing Settler with UniqueId: {UniqueId}, Selected Settler ID: {UnitSelectionManager.SelectedSettlerId}");
            UnitSelectionManager.SetOutline(this, true);
        }
        else
        {
            UnitSelectionManager.SetOutline(this, false);
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsPressed() && UnitSelectionManager.SelectedSettlerId == UniqueId)
        {
            _movementService.StartMovementOnClick(GetGlobalMousePosition());
        }
    }
    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            UnitSelectionManager.SelectUnit(this);
        }
    }

    
}
