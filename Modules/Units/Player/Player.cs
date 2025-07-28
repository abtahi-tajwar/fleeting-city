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
		Vector2 velocity = Vector2.Zero;

		switch (_currentDirection)
		{
			case MOVEMENT_DIRECTION_ENUM.UP:
				velocity = Vector2.Up;
				break;
			case MOVEMENT_DIRECTION_ENUM.DOWN:
				velocity = Vector2.Down;
				break;
			case MOVEMENT_DIRECTION_ENUM.LEFT:
				velocity = Vector2.Left;
				break;
			case MOVEMENT_DIRECTION_ENUM.RIGHT:
				velocity = Vector2.Right;
				break;
			case MOVEMENT_DIRECTION_ENUM.UP_LEFT:
				velocity = (Vector2.Up + Vector2.Left).Normalized();
				break;
			case MOVEMENT_DIRECTION_ENUM.UP_RIGHT:
				velocity = (Vector2.Up + Vector2.Right).Normalized();
				break;
			case MOVEMENT_DIRECTION_ENUM.DOWN_LEFT:
				velocity = (Vector2.Down + Vector2.Left).Normalized();
				break;
			case MOVEMENT_DIRECTION_ENUM.DOWN_RIGHT:
				velocity = (Vector2.Down + Vector2.Right).Normalized();
				break;

		}

		Velocity = velocity * MovementSpeed;
		MoveAndSlide();
		var boundRect = WorldBound.WorldShape.GetRect();

		Position = Position.Clamp(boundRect.Position, boundRect.End);
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
