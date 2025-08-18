using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class EventBus : Node
{
    //Singleton instance
    public static EventBus Instance { get; private set; }
    // Singals
    [Signal]
    public delegate void ActionHoldEventHandler(string actionType, bool value);
    [Signal]
    public delegate void ActionToggleEventHandler(string actionType);
    [Signal]
    public delegate void PlayerMoveEventHandler(string actionType);
    [Signal]
    public delegate void UnitMoveEventHandler(Vector2 touchPos);
    [Signal]
    public delegate void PinchZoomEventHandler(float pinchZoomDelta);
    [Signal]
    public delegate void ScrollZoomEventHandler(int direction);

    // From Interactables
    [Signal]
    public delegate void InteractionZoneEnteredEventHandler(INTERACTION interaction);
    [Signal]
    public delegate void InteractionZoneExitedEventHandler(INTERACTION interaction);
    [Signal]
    public delegate void InteractionPressedEventHandler();
    [Signal]
    public delegate void InteractionCommandEventHandler(INTERACTION interaction);
    [Signal]
    public delegate void SelectedUnitChangedEventHandler();


    public override void _EnterTree()
    {
        if (Instance == null) Instance = this;
    }
    public override void _ExitTree()
    {
        if (Instance == this) Instance = null;
    }

    public void EmitActionHold(ACTION_ENUM actionType, bool value)
    {
        EmitSignal(SignalName.ActionHold, actionType.ToString(), value);
    }
    public void EmitActionToggle(ACTION_ENUM actionType)
    {
        EmitSignal(SignalName.ActionToggle, actionType.ToString());
    }

    public void EmitPlayerMove(MOVEMENT_DIRECTION_ENUM actionType)
    {
        EmitSignal(SignalName.PlayerMove, actionType.ToString());
    }

    public void EmitUnitMove(Vector2 touchPos)
    {
        EmitSignal(SignalName.UnitMove, touchPos);
    }

    public void EmitInteractionZoneEntered(INTERACTION interaction)
    {
        EmitSignal(SignalName.InteractionZoneEntered, interaction.ToString());
    }
    public void EmitInteractionZoneExited(INTERACTION interaction)
    {
        EmitSignal(SignalName.InteractionZoneExited, interaction.ToString());
    }

    public void EmitPinchZoom(float pinchZoomDelta)
    {
        EmitSignal(SignalName.PinchZoom, pinchZoomDelta);
    }
    public void EmitScrollZoom(int direction)
    {
        EmitSignal(SignalName.ScrollZoom, direction);
    }

    public void EmitSelectedUnitChanged()
    {
        EmitSignal(SignalName.SelectedUnitChanged);
    }
    public void EmitInteractionCommand(INTERACTION interactionType)
    {
        EmitSignal(SignalName.InteractionCommand, interactionType.ToString());
    }
    public void EmitInteractionPressed()
    {
        EmitSignal(SignalName.InteractionPressed);
    }
}
