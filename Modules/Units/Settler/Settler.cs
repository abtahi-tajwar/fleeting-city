using FleetingCity.BAL.Model;
using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class Settler : CharacterBody2D
{
	[Export]
	public SelectionArea SelectionArea;

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

		CallDeferred(nameof(ConnectToEventBus));

		SelectionArea.OnSelect += HandleSelect;
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

	private void HandleSettlerMove(Vector2 touchPos)
	{
		if (ActionManager.CurrentAction == ACTION_ENUM.MOVE)
		{
			if (UnitSelectionManager.SelectedSettler != null
			&& UnitSelectionManager.SelectedSettler.Model.Id == Model.Id)
			{
				_movementService.StartMovementOnClick(touchPos);
			}
		}
	}

	private void HandleSelect()
	{
		if (ActionManager.CurrentAction == ACTION_ENUM.POINT)
		{
			_unitSelectionManager.SelectUnit();
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
			EventBus.SignalName.UnitMove,
			new Callable(this, nameof(HandleSettlerMove))
		);
	}
}
