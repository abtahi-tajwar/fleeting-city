using Godot;
using System;

public partial class VirtualJoystick : Control
{
	[Export]
	public TextureRect Inner;
	[Export]
	public TextureRect Outer;
	[Export]
	float MaxClampRadius = 50f; // The maximum distance from center
	[Export]
	public float DEADZONE = 0.1f; // tweak as you like (0..1)

	private bool _pressed;
	private Vector2 _start;
	private Vector2 initialInnerPos;
	private Vector2 initialInnerPosGlobal;
	private Vector2 outerPos;

	public override void _Ready()
	{
		initialInnerPos = Inner.Position;
		initialInnerPosGlobal = Inner.GlobalPosition;
		outerPos = Outer.GlobalPosition;
		GD.Print($"Inner position intial {initialInnerPos}, {initialInnerPosGlobal}");
	}

	public override void _Process(double delta)
	{
		if (!UnitSelectionManager.IsPlayerSelected) Visible = false;
		else Visible = true;
	}
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventScreenTouch t)
		{
			if (t.Pressed)
			{
				_pressed = true;
				GD.Print("Knob pressed");
				_start = GetGlobalMousePosition();
			}
			else
			{
				_pressed = false;
				// reset knob here
				GD.Print("Knob released");
				Inner.Position = initialInnerPos;
				ReleaseAllMoveActions();
			}
		}
		else if (_pressed && @event is InputEventScreenDrag d)
		{
			var mousePos = GetGlobalMousePosition();
			var offset = mousePos - initialInnerPosGlobal;

			// Clamp length
			if (offset.Length() > MaxClampRadius)
				offset = offset.Normalized() * MaxClampRadius;

			Inner.GlobalPosition = initialInnerPosGlobal + offset;
			InvokeDirectionInput();
			GD.Print($"Knob should be moving {mousePos}");
			// clamp & move knob; compute direction, etc.
		}
	}

	private void InvokeDirectionInput()
	{
		// Offset of knob from the center you stored on touch start
		Vector2 offset = Inner.GlobalPosition - initialInnerPosGlobal;

		// Convert to [-1..1] range using your clamp radius
		Vector2 dir = offset / MaxClampRadius;

		// Keep length <= 1
		if (dir.LengthSquared() > 1f)
			dir = dir.Normalized();

		// Deadzone
		if (dir.Length() < DEADZONE)
			dir = Vector2.Zero;

		ReleaseAllMoveActions();

		// Apply strengths (Godot UI Y+ is down)
		if (dir.X > 0f) Input.ActionPress("move_right", Mathf.Clamp(dir.X, 0f, 1f));
		else if (dir.X < 0f) Input.ActionPress("move_left", Mathf.Clamp(-dir.X, 0f, 1f));

		if (dir.Y > 0f) Input.ActionPress("move_down", Mathf.Clamp(dir.Y, 0f, 1f));
		else if (dir.Y < 0f) Input.ActionPress("move_up", Mathf.Clamp(-dir.Y, 0f, 1f));
	}

	private void ReleaseAllMoveActions()
	{
		Input.ActionRelease("move_left");
		Input.ActionRelease("move_right");
		Input.ActionRelease("move_up");
		Input.ActionRelease("move_down");
	}


}
