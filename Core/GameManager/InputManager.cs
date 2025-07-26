using FleetingCity.Enums;
using Godot;
using System;

public partial class InputManager : Node
{
	// Singleton instance
	public static InputManager Instance { get; private set; }
	public bool IsDragging { get; set; } = false;
	public Vector2 MouseMoveDelta = Vector2.Zero;

	// privates
	private Vector2 _lastMousePosition;
	public override void _Ready()
	{
		Instance = this;
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

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.IsPressed())
			{

				IsDragging = true;
				_lastMousePosition = GetViewport().GetMousePosition();
			}
			else if (!mouseButtonEvent.IsPressed())
			{
				IsDragging = false;
			}
		}
		else if (@event is InputEventScreenTouch screenTouchEvent)
		{
			if (screenTouchEvent.IsPressed())
			{

				IsDragging = true;
				_lastMousePosition = GetViewport().GetMousePosition();
			}
			else if (!screenTouchEvent.IsPressed())
			{
				IsDragging = false;
			}
		}
		if (@event is InputEventMouseMotion motion && IsDragging)
		{
			var currentMousePosition = GetViewport().GetMousePosition();
			MouseMoveDelta = currentMousePosition - _lastMousePosition;
			_lastMousePosition = currentMousePosition;
		}

	}
}
