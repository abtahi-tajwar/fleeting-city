using FleetingCity.BAL.Modules;
using FleetingCity.Enums;
using Godot;
using System;

public partial class Settler : CharacterBody2D
{
	// Services
	private CharacterMovementService _movementService;
	private CharacterAnimationService _animationService;
	private UnitSelectionManager _unitSelectionManager;

	// Model
	public SettlerModel Model { get; private set; }

	public override void _Ready()
	{
		Model = new SettlerModel(); // Initialize the model with a unique ID
		_movementService = new CharacterMovementService(this);
		UnitSelectionManager.SetOutline(this, false);

		_animationService = new CharacterAnimationService(this);
		_unitSelectionManager = new UnitSelectionManager(this);
	}
	public override void _PhysicsProcess(double delta)
	{
		MOVEMENT_DIRECTION_ENUM moveDirection = _movementService.Update(delta);
		_animationService.PlayWalkAnimation(moveDirection);
	}
	public override void _Process(double delta)
	{
		// Additional processing logic can be added here if needed
		if (
			UnitSelectionManager.SelectedSettler != null
			&& UnitSelectionManager.SelectedSettler.Model.Id == Model.Id
		)
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
		)
		{
			if (ActionManager.CurrentAction == ACTION_ENUM.MOVE)
			{
				if (UnitSelectionManager.SelectedSettler.Model.Id == Model.Id)
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
