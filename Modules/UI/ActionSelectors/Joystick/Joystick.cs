using Godot;
using System;

public partial class Joystick : Control
{
    private Button _innerStick;

    public override void _Ready()
    {
        _innerStick = GetNode<Button>("InnerStick");
        _innerStick.Pressed += () => HandleJoystickPress();
    }
    public override void _Process(double delta)
    {
        if (UnitSelectionManager.IsPlayerSelected)
        {
            this.Visible = true;
        }
        else
        {
            this.Visible = false;
        }
    }

    private void HandleJoystickPress()
    {
        GD.Print("Joystick pressed");
    }
}
