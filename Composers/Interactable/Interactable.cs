using FleetingCity.BAL.Data;
using FleetingCity.BAL.Enum;
using Godot;
using System;

public partial class Interactable : Node
{
	private Area2D _area;

	// Exports
	[Export]
	public INTERACTION InteractionType;
	[Export]
	public Label HintLabel;

	// Signals
	[Signal]
	public delegate void InteractionStartEventHandler(CharacterBody2D character);
	[Signal]
	public delegate void InteractionEndEventHandler(CharacterBody2D character);

	// Privates
	private string _interactionKeyName;


	public override void _Ready()
	{
		_interactionKeyName = GetActionKeyName("interact");
		foreach (var item in InteractableHintData.Instance.Data)
		{
			GD.Print($"id: {item.Key}, label: {item.Value.Label}");
		}
		if (HintLabel != null || GameManager.IsPlatformMobile) HintLabel.Visible = false;
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

	public void UpdateInteraction(INTERACTION newInteraction)
	{
		InteractionType = newInteraction;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is CharacterBody2D character)
		{
			if (HintLabel != null && !GameManager.IsPlatformMobile)
			{
				HintLabel.Visible = true;
				foreach (var item in InteractableHintData.Instance.Data)
				{
					GD.Print($"id: {item.Key}, label: {item.Value.Label}");
				}
				var hintData = InteractableHintData.Instance.Data[InteractionType.ToString()];
				HintLabel.Text = (hintData != null) ? GetHintLabel(hintData.Label) : GetHintLabel("Interact");
			}
			EmitSignal(SignalName.InteractionStart, character);
			EventBus.Instance.EmitInteractionZoneEntered(InteractionType);
		}
	}
	private void OnBodyExited(Node body)
	{
		if (body is CharacterBody2D character)
		{
			if (HintLabel != null && !GameManager.IsPlatformMobile) HintLabel.Visible = false;
			EmitSignal(SignalName.InteractionEnd, body);
			EventBus.Instance.EmitInteractionZoneExited(InteractionType);
		}
	}

	private string GetActionKeyName(string actionName)
	{
		var events = InputMap.ActionGetEvents(actionName);

		foreach (var e in events)
		{
			if (e is InputEventKey keyEvent)
			{
				return OS.GetKeycodeString(keyEvent.PhysicalKeycode);
			}
		}

		return null; // no key bound
	}

	private string GetHintLabel(string interactionType)
	{
		return $"Press {_interactionKeyName} to {interactionType}";
	}

}
