using FleetingCity.BAL.Modules;
using FleetingCity.Enums;
using Godot;
using System;
using System.Reflection.Metadata;

public partial class Player : CharacterBody2D
{
	private CharacterAnimationService _animationService;
	private UnitSelectionManager _unitSelectionManager;
	[Export]
	public float MovementSpeed; // Speed of the player movement

	// privates
	private MOVEMENT_DIRECTION_ENUM _currentDirection = MOVEMENT_DIRECTION_ENUM.NONE;

	public override void _Ready()
	{
		_animationService = new CharacterAnimationService(this);
		_unitSelectionManager = new UnitSelectionManager(this);
		MovementSpeed = PlayerModel.MovementSpeed;
		GameManager.SetPlayer(this); // Register the player with GameManager

		CallDeferred(nameof(ConnectToEventBus));
	}
	public override void _PhysicsProcess(double delta)
	{
		_animationService.PlayWalkAnimation(_currentDirection);
		Move(delta);
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
			&& ActionManager.CurrentAction == ACTION_ENUM.POINT
		)
		{
			_unitSelectionManager.SelectPointedUnit(GetGlobalMousePosition());
		}

	}

	public void UpdateCurrentDirection(string directionStr)
	{
		_currentDirection = Enum.Parse<MOVEMENT_DIRECTION_ENUM>(directionStr, true);
	}

	private void Move(double delta)
	{
		if (_currentDirection == MOVEMENT_DIRECTION_ENUM.NONE)
		{
			return;
		} else if (_currentDirection == MOVEMENT_DIRECTION_ENUM.UP)
		{
			Position += new Vector2(0, -MovementSpeed * (float)delta);
		}
		else if (_currentDirection == MOVEMENT_DIRECTION_ENUM.DOWN)
		{
			Position += new Vector2(0, MovementSpeed * (float)delta);
		}
		else if (_currentDirection == MOVEMENT_DIRECTION_ENUM.LEFT)
		{
			Position += new Vector2(-MovementSpeed * (float)delta, 0);
		}
		else if (_currentDirection == MOVEMENT_DIRECTION_ENUM.RIGHT)
		{
			Position += new Vector2(MovementSpeed * (float)delta, 0);
		}
	}

	private void ConnectToEventBus()
	{
		if (EventBus.Instance == null)
		{
			GD.PrintErr("EventBus instance is still null even after deferring.");
			return;
		}

		EventBus.Instance.Connect(
			"PlayerMove",
			new Callable(this, nameof(UpdateCurrentDirection))
		);
		GD.Print("Connected to EventBus for PlayerMove events.");
	}
}
