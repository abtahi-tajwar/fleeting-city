using FleetingCity.Enums;
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
}
