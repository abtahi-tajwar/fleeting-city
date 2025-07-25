using Godot;
using System;

public partial class Camera : Node2D
{
    // Privates
    private Node2D _target = null;
    public override void _Process(double delta)
    {
        if (UnitSelectionManager.SelectedSettler != null)
        {
            if (_target != null)
            {
                if (UnitSelectionManager.IsPlayerSelected)
                {
                    _target = GameManager.Player;
                }
                else
                {
                    _target = UnitSelectionManager.SelectedSettler;
                }
            }
            else
            {
                if (UnitSelectionManager.IsPlayerSelected)
                {
                    if (_target != GameManager.Player)
                    {
                        _target = GameManager.Player;
                    }
                }
                else
                {
                    if (_target != UnitSelectionManager.SelectedSettler)
                    {

                        _target = UnitSelectionManager.SelectedSettler;
                    }
                }
            }
        }

        if (_target != null)
        {
            GlobalPosition = _target.GlobalPosition;
        }
    }
}
