using Godot;
using System;

public partial class SelectionArea : Area2D
{
	[Signal]
	public delegate void OnSelectEventHandler();

	// Exports
	[Export]
	public CollisionShape2D Collision;

	public override void _Ready()
	{
		InputPickable = true;
		InputEvent += OnInputEvent;
	}

	private void OnInputEvent(Node viewport, InputEvent e, long shapeIdx)
	{
		if (e is InputEventScreenTouch t)
		{
			if (!t.Pressed) EmitSignal(SignalName.OnSelect);
		}
		if (e is InputEventMouseButton mb)
		{
			if (!mb.Pressed) EmitSignal(SignalName.OnSelect);
		}

	}


}
