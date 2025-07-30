using FleetingCity.BAL.Enums;
using Godot;
using System;

public partial class InputManager : Node2D
{
	// Singleton instance
	public static InputManager Instance { get; private set; }
	public bool IsDragging { get; set; } = false;
	private bool IsMousePressed = false;
	private bool DraggingJustFinished = false;
	public Vector2 MouseDragDelta = Vector2.Zero;

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
		CapturePlayerMovementInput(delta);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed)
			{
				OnMouseDown();
			}
			else
			{
				OnMouseUp();
			}
		}
		else if (@event is InputEventScreenTouch touchEvent)
		{
			if (touchEvent.Pressed)
			{
				OnMouseDown();
			}
			else
			{
				OnMouseDown();
			}
		}


		// Detect motion
		if (@event is InputEventMouseMotion motion && IsMousePressed)
		{
			if (IsMousePressed)
			{
				IsDragging = true;
				// Capture mouse movement delta
				var currentMousePosition = GetViewport().GetMousePosition();
				MouseDragDelta = currentMousePosition - _lastMousePosition;
				_lastMousePosition = currentMousePosition;
			}
			// Don't reset dragging here
		}

	}

	private void OnMouseDown()
	{
		IsMousePressed = true;
		_lastMousePosition = GetViewport().GetMousePosition();
	}
	private void OnMouseUp()
	{
		IsMousePressed = false;
		if (!IsDragging)
		{
			EventBus.Instance.EmitSettlerSelectOrMove(GetGlobalMousePosition());
			GD.Print("Move settler please");
		}
		IsDragging = false;
	}

	private void CapturePlayerMovementInput(double delta)
	{
		Vector2 vec = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		MOVEMENT_DIRECTION_ENUM newDir = MOVEMENT_DIRECTION_ENUM.NONE;

		if (vec.LengthSquared() > 0.01f) // Ignore slight stick noise
		{
			float angle = vec.Angle(); // Radians

			// Convert angle into direction
			if (angle >= -Mathf.Pi / 8 && angle < Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.RIGHT;
			else if (angle >= Mathf.Pi / 8 && angle < 3 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.DOWN_RIGHT;
			else if (angle >= 3 * Mathf.Pi / 8 && angle < 5 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.DOWN;
			else if (angle >= 5 * Mathf.Pi / 8 && angle < 7 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.DOWN_LEFT;
			else if (angle >= 7 * Mathf.Pi / 8 || angle < -7 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.LEFT;
			else if (angle >= -7 * Mathf.Pi / 8 && angle < -5 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.UP_LEFT;
			else if (angle >= -5 * Mathf.Pi / 8 && angle < -3 * Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.UP;
			else if (angle >= -3 * Mathf.Pi / 8 && angle < -Mathf.Pi / 8)
				newDir = MOVEMENT_DIRECTION_ENUM.UP_RIGHT;
		}

		if (newDir != _lastDirection)
		{
			_lastDirection = newDir;
			EventBus.Instance.EmitPlayerMove(newDir);
		}
	}

}
