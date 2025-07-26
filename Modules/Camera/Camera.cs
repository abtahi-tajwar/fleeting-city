using Godot;
using System;

public partial class Camera : Node2D
{
    // Exports
    [Export]
    public float CameraSpeed = 0.5f;
    // Privates
    private Node2D _target = null;
    private bool _followTarget = true;
    public override void _Ready()
    {
        CallDeferred(nameof(SetDefaultTargetToPlayer));
    }

    public override void _Process(double delta)
    {
        if (_followTarget)
        {
            // Smoothly follow the target
            Position = _target != null ? _target.GlobalPosition : Position;
        }
        if (InputManager.Instance.IsDragging)
        {
            _followTarget = false;
            Position -= InputManager.Instance.MouseMoveDelta * CameraSpeed; // Subtract to simulate camera drag
            InputManager.Instance.MouseMoveDelta = Vector2.Zero; // Reset the delta after applying it
        }
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
