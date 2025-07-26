using Godot;
using System;

public partial class Camera : Node2D
{
    // Privates
    private Node2D _target = null;
    public override void _Ready()
    {
        CallDeferred(nameof(SetDefaultTargetToPlayer));
    }

    public override void _Process(double delta)
    {
        Position = _target != null ? _target.GlobalPosition : Position;
    }

    private void SetDefaultTargetToPlayer()
    {
        if (GameManager.Player != null)
        {
            _target = GameManager.Player;
        }
        else
        {
            GD.PrintErr("Player is not set in GameManager. Cannot set camera target.");
        }
    }
}
