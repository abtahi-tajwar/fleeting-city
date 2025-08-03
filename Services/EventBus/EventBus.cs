using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class EventBus : Node
{
    //Singleton instance
    public static EventBus Instance { get; private set; }
    // Singals
    [Signal]
    public delegate void ActionChangeEventHandler(string actionType, bool value);
    [Signal]
    public delegate void PlayerMoveEventHandler(string actionType);
    [Signal]
    public delegate void SettlerSelectOrMoveEventHandler(Vector2 touchPos);

    // From Interactables
    [Signal]
    public delegate void InteractionZoneEnteredEventHandler(INTERACTION interaction);
    [Signal]
    public delegate void InteractionZoneExitedEventHandler(INTERACTION interaction);


    public override void _Ready()
    {
        Instance = this;
    }
    public void EmitActionChange(ACTION_ENUM actionType, bool value)
    {
        EmitSignal(SignalName.ActionChange, actionType.ToString(), value);
    }

    public void EmitPlayerMove(MOVEMENT_DIRECTION_ENUM actionType)
    {
        EmitSignal(SignalName.PlayerMove, actionType.ToString());
    }

    public void EmitSettlerSelectOrMove(Vector2 touchPos)
    {
        EmitSignal(SignalName.SettlerSelectOrMove, touchPos);
    }

    public void EmitInteractionZoneEntered(INTERACTION interaction)
    {
        EmitSignal(SignalName.InteractionZoneEntered, interaction.ToString());
    }
    public void EmitInteractionZoneExited(INTERACTION interaction)
    {
        EmitSignal(SignalName.InteractionZoneExited, interaction.ToString());
    }
    
}
