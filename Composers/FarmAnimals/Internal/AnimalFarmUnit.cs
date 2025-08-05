using Godot;
using System;

public partial class AnimalFarmUnit : Node2D
{
	public Godot.Timer Timer;

	[Signal]
	public delegate void MovementTimerTimoutEventHandler();

	public override void _Ready()
	{
		Timer = GetNodeOrNull<Godot.Timer>("MovementTimer");
		if (Timer == null) throw new Exception("Please add Timer named MovementTimer for farm unit to move");
		Timer.Timeout += () =>
		{
			EmitSignal(SignalName.MovementTimerTimout);
		};
	}
	
	
}
