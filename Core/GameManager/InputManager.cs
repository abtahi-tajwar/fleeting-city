using FleetingCity.Enums;
using Godot;
using System;

public partial class InputManager : Node
{
	public override void _Ready()
	{
		CallDeferred(nameof(ConnectToEventBus));
	}

	private void ConnectToEventBus()
	{
		if (EventBus.Instance == null)
		{
			GD.PrintErr("EventBus instance is still null even after deferring.");
			return;
		}
	}

	private MOVEMENT_DIRECTION_ENUM _lastDirection = MOVEMENT_DIRECTION_ENUM.NONE;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 vec = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		MOVEMENT_DIRECTION_ENUM newDir = MOVEMENT_DIRECTION_ENUM.NONE;

		if (vec.LengthSquared() > 0.01f) // ignore slight stick noise
		{
			if (Mathf.Abs(vec.X) > Mathf.Abs(vec.Y))
			{
				newDir = vec.X > 0 ? MOVEMENT_DIRECTION_ENUM.RIGHT : MOVEMENT_DIRECTION_ENUM.LEFT;
			}
			else
			{
				newDir = vec.Y > 0 ? MOVEMENT_DIRECTION_ENUM.DOWN : MOVEMENT_DIRECTION_ENUM.UP;
			}
		}

		if (newDir != _lastDirection)
		{
			_lastDirection = newDir;
			EventBus.Instance.EmitPlayerMove(newDir);
		}
	}
}
