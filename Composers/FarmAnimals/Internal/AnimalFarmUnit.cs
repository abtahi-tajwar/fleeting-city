using Godot;
using System;

public partial class AnimalFarmUnit : CharacterBody2D
{
	public Godot.Timer MovementTimer;

	[Signal]
	public delegate void MovementTimerTimoutEventHandler();

	// privates
	private AnimalFarmUnitMovementService _movementService;
	private CharacterAnimationService _animationService;
	private TileMapLayer _tileMapLayer;

	// Debug
	private Vector2 tempPos;



	public void Setup(TileMapLayer tileMapLayer)
	{
		_tileMapLayer = tileMapLayer;
		_movementService = new AnimalFarmUnitMovementService(this, _tileMapLayer);
		_animationService = new CharacterAnimationService(this);
	}
	public override void _Ready()
	{
		MovementTimer = GetNodeOrNull<Godot.Timer>("MovementTimer");
		if (MovementTimer == null) throw new Exception("Please add Timer named MovementTimer for farm unit to move");
		MovementTimer.Timeout += () =>
		{
			EmitSignal(SignalName.MovementTimerTimout);
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_movementService == null)
		{
			// GD.Print("Movement service is not defined");
			return;
		}
		var dir = _movementService.Update(delta);
		_animationService.PlayWalkAnimation(dir);
	}

	public void HandleMove(Vector2 Position)
	{
		MovementTimer.WaitTime = GD.RandRange(5, 7);
		tempPos = Position;
		_movementService.StartMovementTo(Position);
	}

	
	
}
