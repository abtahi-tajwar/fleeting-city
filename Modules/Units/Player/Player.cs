using FleetingCity.BAL.Model;
using FleetingCity.BAL.Enum;
using Godot;
public partial class Player : CharacterBody2D
{
	[Export]
	public float MovementSpeed; // Speed of the player movement
	[Export]
	public SelectionArea SelectionArea;

	// publics
	public bool IsInteracting { get; set; } = false;
	public INTERACTION? CurrentInteractionType { get; set; } = null;

	// privates
	private CharacterAnimationService _animationService;
	private UnitSelectionManager _unitSelectionManager;
	private MOVEMENT_DIRECTION_ENUM _currentDirection = MOVEMENT_DIRECTION_ENUM.NONE;

	public override void _Ready()
	{
		_animationService = new CharacterAnimationService(this);
		_unitSelectionManager = new UnitSelectionManager(this);
		MovementSpeed = PlayerModel.MovementSpeed;
		GameManager.SetPlayer(this); // Register the player with GameManager

		CallDeferred(nameof(ConnectToEventBus));

		SelectionArea.OnSelect += OnSelect;
	}
	public override void _PhysicsProcess(double delta)
	{
		if (!IsInteracting)
		{
			_animationService.PlayWalkAnimation(_currentDirection);
			Move(delta);
		}
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

	private void OnSelect()
	{
		if (ActionManager.CurrentAction == ACTION_ENUM.POINT)
		{
			_unitSelectionManager.SelectUnit();
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

	private void OnInteractionCommand(string interactionStr, Node2D sender)
	{
		var interactionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interactionStr, true);
		IsInteracting = true;
		CurrentInteractionType = interactionType;
	}
	private void OnInteractionStopCommand(string interactionStr)
	{
		var interactionType = (INTERACTION)Enum.Parse(typeof(INTERACTION), interactionStr, true);
		IsInteracting = false;
		CurrentInteractionType = null;

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

		EventBus.Instance.Connect(
			EventBus.SignalName.InteractionCommand,
			new Callable(this, nameof(OnInteractionCommand))
		);
		EventBus.Instance.Connect(
			EventBus.SignalName.InteractionStopCommand,
			new Callable(this, nameof(OnInteractionStopCommand))
		);

		// EventBus.Instance.Connect(
		// 	EventBus.SignalName.UnitSelectOrMove,
		// 	new Callable(this, nameof(OnSelect))
		// );
		// GD.Print("Connected to EventBus for PlayerMove events.");
	}
}
