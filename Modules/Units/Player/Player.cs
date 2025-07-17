using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private CharacterMovementService _movementService;

	public override void _Ready()
	{
		_movementService = new CharacterMovementService(this);
	}
	public override void _PhysicsProcess(double delta)
	{
		_movementService.Update(delta);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsPressed())
		{
			_movementService.StartMovementOnClick(GetGlobalMousePosition());
		}

	}
}
