using Godot;
using System;

public partial class Camera : Node2D
{
    // Exports
    [Export]
    public float CameraSpeed = 50f;
    // Privates
    private Node2D _target = null;
    private bool _followTarget = true;
    public override void _Ready()
    {
        CallDeferred(nameof(SetDefaultTargetToPlayer));
        CallDeferred(nameof(ConnectToEventBus));

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

            // Instead of scaling delta directly, normalize it and scale manually
            Vector2 deltaTime = InputManager.Instance.MouseDragDelta;

            if (deltaTime.Length() > 0.01f)
            {
                var boundRect = WorldBound.WorldShape.GetRect();
                // Normalize direction but keep consistent movement speed
                Vector2 direction = deltaTime.Normalized();

                // Apply fixed speed in that direction (scale with delta to simulate drag)
                Vector2 newPosition = new Vector2(
                    direction.X * CameraSpeed * deltaTime.Length() * (float)delta,
                    direction.Y * CameraSpeed * deltaTime.Length() * (float)delta
                );
                Position -= newPosition;
                Position = Position.Clamp(boundRect.Position, boundRect.End);
            }

            // Reset delta
            InputManager.Instance.MouseDragDelta = Vector2.Zero;

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

    public void RefocusToPlayer(string _)
    {
        _followTarget = true;
    }
    private void ConnectToEventBus()
    {
        if (EventBus.Instance == null)
        {
            GD.PrintErr("EventBus instance is still null even after deferring.");
            return;
        }

        EventBus.Instance.Connect(
            "PlayerMove",
            new Callable(this, nameof(RefocusToPlayer))
        );
        GD.Print("Connected to EventBus for PlayerMove events from camera.");
    }
}
