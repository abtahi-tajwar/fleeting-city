using FleetingCity.Enums;
using Godot;
using System;

public partial class EventBus : Node
{
    //Singleton instance
    public static EventBus Instance { get; private set; }
    // Singals
    [Signal]
    public delegate void ActionChangeEventHandler(string actionType);
    
    public override void _Ready()
    {
        Instance = this;
    }
    public void EmitActionChange(ACTION_ENUM actionType, bool value)
    {
        EmitSignal(SignalName.ActionChange, actionType.ToString(), value);
    }
}
