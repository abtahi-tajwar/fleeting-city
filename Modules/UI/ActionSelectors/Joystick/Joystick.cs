using Godot;
using System;

public partial class Joystick : Control
{
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
}
