using Godot;
using System;

public partial class EventBus : Node
{
    [Signal]
    public delegate void ActionChangeEventHandler(string actionType);

    public void EmitActionChange(string actionType)
    {
        EmitSignal(SignalName.ActionChange, actionType);
    }
}
