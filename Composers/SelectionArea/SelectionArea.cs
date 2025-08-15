using Godot;
using System;
using System.Diagnostics;

public partial class SelectionArea : Area2D
{
	[Signal]
	public delegate void OnSelectEventHandler();

	// Exports
	[Export]
	public CollisionShape2D Collision;
	private RectangleShape2D _collisionShape;
	public override void _Ready()
	{
		_collisionShape = Collision.Shape as RectangleShape2D;
		if (_collisionShape == null) GD.PrintErr("Collision shape is not a RectangleShape2D");
		InputPickable = true;
		InputEvent += OnInputEvent;
	}


	private void OnInputEvent(Node viewport, InputEvent e, long shapeIdx)
	{
		if (e is InputEventScreenTouch t)
		{
			// if (!t.Pressed) EmitSignal(SignalName.OnSelect);
		}
		if (e is InputEventMouseButton mb)
		{
			// if (!mb.Pressed) EmitSignal(SignalName.OnSelect);
		}

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton pointer)
		{
			// RectangleShape2D is centered; compute local top-left
			var worldP = Helper.CalculateGlobalPointerPosition(this, pointer.Position);
			var hit = PhysicsHelper.PointOverlapsCollisionRect(worldP, Collision);

			if (hit)
			{
				if (pointer.Pressed) EmitSignal(SignalName.OnSelect);
			}

		}
		else if (@event is InputEventScreenTouch finger)
		{
						// RectangleShape2D is centered; compute local top-left
			var worldP = Helper.CalculateGlobalPointerPosition(this, finger.Position);
			var hit = PhysicsHelper.CircleOverlapsCollisionRect(worldP, InputManager.Instance.TouchFingerRadius, Collision);

			if (hit)
			{
				if (finger.Pressed) EmitSignal(SignalName.OnSelect);
			}
		}
	}


}
