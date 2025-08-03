using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class Interactable : Node
{
	private Area2D _area;

	// Exports
	[Export]
	public INTERACTION InteractionType;

	// Signals
	[Signal]
	public delegate void InteractionStartEventHandler(CharacterBody2D character);
	[Signal]
	public delegate void InteractionEndEventHandler(CharacterBody2D character);


	public override void _Ready()
	{
		_area = GetNode<Area2D>("InteractBoundary");
		if (_area == null)
		{
			GD.PrintErr("Area2D node not found in Interactable.");
		}
		else
		{
			_area.BodyEntered += OnBodyEntered;
			_area.BodyExited += OnBodyExited;
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body is CharacterBody2D character)
		{
			EmitSignal(SignalName.InteractionStart, character);
			EventBus.Instance.EmitInteractionZoneEntered(InteractionType);
		}
	}
	private void OnBodyExited(Node body)
	{
		if (body is CharacterBody2D character)
		{
			EmitSignal(SignalName.InteractionEnd, body);
			EventBus.Instance.EmitInteractionZoneExited(InteractionType);
		}
	}
}
