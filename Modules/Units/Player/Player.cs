using FleetingCity.Enums;
using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private CharacterMovementService _movementService;
	private CharacterAnimationService _animationService;
	private UnitSelectionManager _unitSelectionManager;

	public override void _Ready()
	{
		_movementService = new CharacterMovementService(this);
		_animationService = new CharacterAnimationService(this);
		_unitSelectionManager = new UnitSelectionManager(this);
		GameManager.SetPlayer(this); // Register the player with GameManager
	}
	public override void _PhysicsProcess(double delta)
	{
		MOVEMENT_DIRECTION_ENUM movementDirection = _movementService.Update(delta);
		_animationService.PlayWalkAnimation(movementDirection);
	}
	public override void _Process(double delta)
	{
		// Additional processing logic can be added here if needed
		if (UnitSelectionManager.IsPlayerSelected)
		{
			UnitSelectionManager.SetOutline(this, true);
		}
		else
		{
			UnitSelectionManager.SetOutline(this, false);
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (
			@event is InputEventMouseButton mouseButtonEvent
			&& mouseButtonEvent.IsPressed()
			// && UnitSelectionManager.IsPlayerSelected
			// && ActionManager.CurrentAction == ACTION_ENUM.MOVE
		)
		{
			if (ActionManager.CurrentAction == ACTION_ENUM.MOVE)
			{
				if (UnitSelectionManager.IsPlayerSelected)
				{
					_movementService.StartMovementOnClick(GetGlobalMousePosition());
				}
			}
			else
			{
				_unitSelectionManager.SelectPointedUnit(GetGlobalMousePosition());
			}

		}

	}
}
