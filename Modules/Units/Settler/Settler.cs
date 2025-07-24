using FleetingCity.Enums;
using Godot;
using System;

public partial class Settler : CharacterBody2D
{
	public string UniqueId { get; set; }

	// Services
	private CharacterMovementService _movementService;
	private CharacterAnimationService _animationService;
	private UnitSelectionManager _unitSelectionManager;

	public override void _Ready()
	{
		UniqueId = Guid.NewGuid().ToString(); // Assign a unique ID to the settler
		GD.Print($"Settler created with UniqueId: {UniqueId}");
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
		if (UnitSelectionManager.SelectedSettlerId == UniqueId)
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
			// && UnitSelectionManager.SelectedSettlerId == UniqueId
			// && ActionManager.CurrentAction == ACTION_ENUM.MOVE
		)
		{
			if (ActionManager.CurrentAction == ACTION_ENUM.MOVE)
			{
				if (UnitSelectionManager.SelectedSettlerId == UniqueId)
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
